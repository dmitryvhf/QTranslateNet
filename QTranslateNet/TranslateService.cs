using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

using Microsoft.Net.Http.Headers;

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
        /// <param name="currentTranslateService">Выбранный сервис перевода</param>
        /// <param name="originalText">Перводимый текст</param>
        /// <param name="langFrom">Язык переводимого текста</param>
        /// <param name="langTo">Язык перевода</param>
        /// <param name="statusLabelControl">Контрол вывода служебных сообщений</param>
        /// <param name="resultTextBoxControl">Текстовый контрол вывода результата перевода</param>
        /// <exception cref="UnreachableException">Неизвестный тип запроса</exception>
        /// <exception cref="ArgumentException">Не заполнено тело HttpPost запроса</exception>
        public static void Translate(
            ITranslateService currentTranslateService,
            String originalText,
            String langFrom,
            String langTo,
            ToolStripStatusLabel statusLabelControl,
            TextBox resultTextBoxControl)
        {
            // Step: prepare request: models and http client
            RequestData request = currentTranslateService.ServiceTranslateRequest(
                originalText, langFrom, langTo);

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
                if (item.Key == HeaderNames.ContentType)
                {
                    continue;
                }

                httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            // Step: translate request by API
            statusLabelControl.Text = "Translating...";

            HttpResponseMessage? response;

            // TODO try-catch
            switch (request.Method)
            {
                case RequestHttpMethodType.HttpGet:
                    response = httpClient.GetAsync(request.RelativeUrl).Result;
                    break;
                case RequestHttpMethodType.HttpPost:
                    if (request.Body == null)
                    {
                        throw new ArgumentException("[ERR] Не заполнено тело запроса");
                    }

                    response = httpClient.PostAsync(request.RelativeUrl, request.Body).Result;
                    request.Body.Dispose();
                    break;
                default:
                    throw new UnreachableException();
            }

            handler.Dispose();
            httpClient.Dispose();

            // Error request validation
            if (response.StatusCode != HttpStatusCode.OK)
            {
                statusLabelControl.Text = "[ERR] Запрос вернулся с ошибкой: " + response.StatusCode + $"[{(int)response.StatusCode}]";
                resultTextBoxControl.Text = response.Content.ReadAsStringAsync().Result;

                response.Dispose();
                return;
            }

            // Step: response parsing
            statusLabelControl.Text = "Parsing...";
            resultTextBoxControl.Text = String.Empty;

            ResponseData responseData = currentTranslateService.ServiceTranslateResponse(response, langFrom, langTo);
            response.Dispose();

            // Step: output result
            resultTextBoxControl.Text = responseData.Text;
            statusLabelControl.Text = String.Empty;
        }
    }
}