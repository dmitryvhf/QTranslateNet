using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

using Microsoft.Net.Http.Headers;

using QTranslateNet.Core;
using QTranslateNet.Core.Helpers;
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
            String baseAddress = currentTranslateService.GetServiceHost(originalText, langFrom, langTo);

            #region Prepare request

            // Prepare request
            if (currentTranslateService.ServiceTranslateBootstrapRequest(originalText, langFrom, langTo, out RequestData? prepareTranslateRequest))
            {
                HttpResponseMessage prepareTranslateResponse = ServiceRequest(prepareTranslateRequest, baseAddress, statusLabelControl);

                if (prepareTranslateResponse.StatusCode != HttpStatusCode.OK)
                {
                    statusLabelControl.Text = "[ERR] Запрос вернулся с ошибкой: " + prepareTranslateResponse.StatusCode + $"[{(int)prepareTranslateResponse.StatusCode}]";
                    // resultTextBoxControl.Text = prepareTranslateResponse.Content.ReadAsStringAsync().Result;

                    prepareTranslateResponse.Dispose();
                    return;
                }

                // Step: response parsing
                statusLabelControl.Text = "Prepare...";
                resultTextBoxControl.Text = String.Empty;

                if (!currentTranslateService.ServiceTranslateBootstrapResponse(prepareTranslateResponse, langFrom, langTo))
                {
                    statusLabelControl.Text = "[ERR] Запрос вернулся с ошибкой: " + prepareTranslateResponse.StatusCode + $"[{(int)prepareTranslateResponse.StatusCode}]";
                    // resultTextBoxControl.Text = prepareTranslateResponse.Content.ReadAsStringAsync().Result;

                    prepareTranslateResponse.Dispose();
                    return;
                }

                prepareTranslateResponse.Dispose();

                // Step: output result
                statusLabelControl.Text = String.Empty;
                //resultTextBoxControl.Text = prepareResponseData.Text;
            }

            #endregion

            #region Autodetect language

            if (langFrom == MyConstants.AutoDetectLanguage.Code
                && currentTranslateService.GetServiceHeader().Capabilities.Contains(Capability.DetectLanguage))
            {
                RequestData autodetectRequest = currentTranslateService.ServiceDetectLanguageRequest(originalText);

                statusLabelControl.Text = "Autodetect language...";
                resultTextBoxControl.Text = String.Empty;

                HttpResponseMessage translateResponse = ServiceRequest(autodetectRequest, baseAddress, statusLabelControl);

                String autoDetectLanguageResult = currentTranslateService.ServiceDetectLanguageResponse(translateResponse);

                if (!String.IsNullOrWhiteSpace(autoDetectLanguageResult))
                {
                    langFrom = autoDetectLanguageResult;
                }
            }

            #endregion

            #region Translate request

            // Get request model
            List<HttpResponseMessage> translateResponses = new List<HttpResponseMessage>();
            RequestData[] translateRequests = currentTranslateService.ServiceTranslateRequest(originalText, langFrom, langTo);

            foreach (RequestData translateRequest in translateRequests)
            {
                HttpResponseMessage translateResponse = ServiceRequest(translateRequest, baseAddress, statusLabelControl);
                if (translateResponse.StatusCode != HttpStatusCode.OK)
                {
                    statusLabelControl.Text = "[ERR] Запрос вернулся с ошибкой: " + translateResponse.StatusCode + $"[{(int)translateResponse.StatusCode}]";
                    resultTextBoxControl.Text = translateResponse.Content.ReadAsStringAsync().Result;

                    for (int i = 0; i < translateResponses.Count; i++)
                    {
                        translateResponses[i].Dispose();
                    }

                    foreach (HttpResponseMessage translateResponseForDispose in translateResponses)
                    {
                        translateResponseForDispose.Dispose();
                    }
                    return;
                }

                translateResponses.Add(translateResponse);

            }

            // Step: response parsing
            statusLabelControl.Text = "Parsing...";
            resultTextBoxControl.Text = String.Empty;

            ResponseData translateResponseData = currentTranslateService.ServiceTranslateResponse(translateResponses.ToArray(), langFrom, langTo);
            for (int i = 0; i < translateResponses.Count; i++)
            {
                translateResponses[i].Dispose();
            }

            // Step: output result
            resultTextBoxControl.Text = translateResponseData.Text;
            statusLabelControl.Text =
                $"{currentTranslateService.GetServiceHeader().Name} > {CommonMethods.LanguageFromCode(langFrom)} to {CommonMethods.LanguageFromCode(langTo)}";

            #endregion
        }

        /// <summary>
        ///     Отправить API запрос с указанными настройками
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="baseAddress">Базовый адрес сервиса</param>
        /// <param name="statusLabelControl">Контрол вывода служебных сообщений</param>
        /// <exception cref="UnreachableException">Неизвестный тип запроса</exception>
        /// <exception cref="ArgumentException">Не заполнено тело HttpPost запроса</exception>
        private static HttpResponseMessage ServiceRequest(
            RequestData request,
            String baseAddress,
            ToolStripStatusLabel statusLabelControl)
        {
            // Step: prepare request: models and http client
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
                // Timeout = TimeSpan.FromSeconds(MyConstants.TimeoutSeconds),
                BaseAddress = new Uri(baseAddress),
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

            // Step: request by API
            statusLabelControl.Text = "Request...";

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

            return response;
        }
    }
}