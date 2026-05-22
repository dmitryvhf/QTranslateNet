using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Web;

using Microsoft.Net.Http.Headers;

using QTranslateNet.Core;
using QTranslateNet.Core.Helpers;
using QTranslateNet.Core.Infrastructure;
using QTranslateNet.Core.Models;

namespace YandexTranslateServiceLibrary
{
    /// <summary>
    ///     Сервис перевода текста "Yandex Translate"
    /// </summary>
    public class YandexTranslateService : TranslateServiceBase
    {
        #region Private consts

        private const Int32 YANDEX_MAX_SOURCE_LEN = 10000;
        private const Int32 YANDEX_EMERGENCY_CHUNK_LEN = 180;
        // private const Int32 YANDEX_MAX_CHUNK_LEN = 600;
        // private const Int32 YANDEX_MAX_RETRY_PER_CHUNK = 2;

        #endregion

        #region TranslateServiceBase implementation

        /// <inheritdoc/>
        protected override String BaseUrlApi => "https://translate.yandex.net";

        /// <inheritdoc/>
        protected override String BaseUrlWeb => "https://translate.yandex.com";
        // private const String BaseUrlTtsHost = "https://tts.voicetech.yandex.net";

        /// <inheritdoc/>
        protected override String Name => "Yandex Translate";

        /// <inheritdoc/>
        protected override String AccessibleName => "Yandex";

        /// <inheritdoc/>
        protected override String Info => "Translate Russian, Spanish, German, French and a number of other languages to and from English."
            + " You can translate individual words, as well as whole texts and webpages."
            + MyConstants.NLine2 + BaseUrlWeb + MyConstants.NLine2
            + "\u00a9 2011-2022 \u00abYandex\u00bb";

        /// <inheritdoc/>
        protected override byte[] ServiceIco => YandexTranslateResource.ServiceIco;

        /// <inheritdoc/>
        protected override Сapability[] Сapabilities => new Сapability[] { Сapability.Translate };

        /// <inheritdoc/>
        public override String GetServiceHost(string langFrom, string langTo, string text)
        {
            // return langFrom === Capability.LISTEN ? YANDEX_TTS_HOST : YANDEX_TRANSLATE_HOST;
            return BaseUrlApi;
        }

        /// <inheritdoc/>
        public override String GetServiceLink(string langFrom, string langTo, string text)
        {
            return BaseUrlWeb + $"/?lang={langFrom}-{langTo}&text={text}";
        }

        /// <inheritdoc/>
        public override RequestData[] ServiceTranslateRequest(string text, string langFrom, string langTo)
        {
            // Get body
            String YandexAppId1 = Guid.NewGuid().ToString().Replace("-", "").ToLowerInvariant();

            string url = $"/api/v1/tr.json/translate?"
                + $"uuid={YandexAppId1}"
                + "&srv=android"
                + $"&lang={langFrom}-{langTo}"
                + "&reason=auto"
                + "&format=text"
                + "&options=4"
                + "&yu=2210680511641235828";

            // Post body
            Dictionary<string, string> requestHeaders = CommonMethods.HttpDefaultHeaders();
            _ = requestHeaders.Remove(HeaderNames.AcceptEncoding);

            requestHeaders.Add(HeaderNames.AcceptEncoding, "identity");
            requestHeaders.Add(HeaderNames.Referer, BaseUrlWeb);
            requestHeaders.Add(HeaderNames.Origin, BaseUrlWeb);
            requestHeaders.Add(HeaderNames.XRequestedWith, "XMLHttpRequest");
            requestHeaders.Add(HeaderNames.Connection, "keep-alive");

            List<RequestData> requests = new List<RequestData>();
            String limitedText = CommonMethods.LimitSource(text, YANDEX_MAX_SOURCE_LEN);
            String[] chunchedText = CommonMethods.ChunkByWordLimit(limitedText, YANDEX_EMERGENCY_CHUNK_LEN).ToArray();

            foreach (string requestText in chunchedText)
            {
                HttpContent content = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["text"] = requestText
                });

                requests.Add(new RequestData()
                {
                    RelativeUrl = url,
                    Method = RequestHttpMethodType.HttpPost,
                    Body = content,
                    Headers = requestHeaders
                });
            }

            return requests.ToArray();
        }

        /// <inheritdoc/>
        public override ResponseData ServiceTranslateResponse(HttpResponseMessage[] responses, string langFrom, string langTo)
        {
            String resultTranslatedText = String.Empty;

            foreach (HttpResponseMessage responseText in responses)
            {
                string json = responseText.Content.ReadAsStringAsync().Result;

                YandexResponse yandexResponse = responseText.Content.ReadFromJsonAsync<YandexResponse>().Result!;

                // TODO Определять конечный символ. Если не "." то можно ставить пробел? А когда переносы строки?
                resultTranslatedText += String.Join("\n", yandexResponse.Text.First()) + " ";
            }

            return new ResponseData()
            {
                Text = resultTranslatedText.Trim(),
                From = langFrom,
                To = langTo
            };
        }

        #endregion
    }
}
