using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

using QTranslateNet.Core;
using QTranslateNet.Core.Infrastructure;
using QTranslateNet.Core.Models;

namespace QTranslateNet
{
    internal static class TranslateService
    {
        /// <summary>
        ///     Выполнить перевод с указанными настройками
        /// </summary>
        /// <param name="currentTranslateService"></param>
        /// <returns></returns>
        /// <exception cref="UnreachableException"></exception>
        public static void Translate(
            ITranslateService currentTranslateService,
            String originalText,
            String langFrom,
            String langTo,
            ToolStripStatusLabel statusLabelControl,
            Control resultTextBoxControl)
        {
            // Step: prepare request: models and http client
            RequestData request = currentTranslateService.ServiceTranslateRequest(
                originalText, langFrom, langTo);

            HttpResponseMessage? response;

            statusLabelControl.Text = "Translating...";

            SocketsHttpHandler handler = new SocketsHttpHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            if (request.SslProtocols != null)
            {
                handler.SslOptions.EnabledSslProtocols = request.SslProtocols.Value;
            }

            HttpClient httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(MyConstants.TimeoutSeconds),
                BaseAddress = new Uri(currentTranslateService.GetServiceHost(originalText, langFrom, langTo)),
            };

            foreach (KeyValuePair<string, string> item in request.Headers)
            {
                // Content-Type должен придти с HttpContent
                if (item.Key == "Content-Type")
                {
                    continue;
                }

                httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            // TODO try-catch
            // Step: translate request by API
            switch (request.Method)
            {
                case RequestHttpMethodType.HttpGet:
                    response = httpClient.GetAsync(request.RelativeUrl).Result;
                    break;
                case RequestHttpMethodType.HttpPost:
                    response = httpClient.PostAsync(request.RelativeUrl, request.Body).Result;
                    break;
                default:
                    throw new UnreachableException();
            }

            //handler.Dispose();
            //httpClient.Dispose();

            // Error request validation
            if (response.StatusCode != HttpStatusCode.OK)
            {
                statusLabelControl.Text = "[ERR] Request error with status " + response.StatusCode + $"[{(int)response.StatusCode}]";
                resultTextBoxControl.Text = response.Content.ReadAsStringAsync().Result;

                return;
            }

            // Step: response parsing
            statusLabelControl.Text = "Parsing...";
            resultTextBoxControl.Text = String.Empty;

            ResponseData responseData = currentTranslateService.ServiceTranslateResponse(response, langFrom, langTo);

            // Step: output result
            resultTextBoxControl.Text = responseData.Text;
            statusLabelControl.Text = String.Empty;
        }
    }
}
