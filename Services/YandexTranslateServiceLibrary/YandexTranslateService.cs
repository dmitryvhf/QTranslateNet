using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

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
        public override RequestData ServiceTranslateRequest(string text, string langFrom, string langTo)
        {
            // Get body
            String YandexAppId1 = Guid.NewGuid().ToString().Replace("-", "").ToLowerInvariant();

            string url = $"/api/v1/tr.json/translate?"
                + $"uuid={YandexAppId1}"
                + "&srv=android"
                + $"&lang={langFrom}-{langTo}"
                + "&reason=auto"
                + "&format=text"
                + "&yu=2210680511641235828";

            // Post body
            HttpContent content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["text"] = CommonMethods.EncodePostParam(text)
            });

            Dictionary<string, string> requestHeaders = CommonMethods.HttpDefaultHeaders();
            requestHeaders.Add(HeaderNames.Referer, "https://translate.yandex.com");

            return new RequestData()
            {
                RelativeUrl = url,
                Method = RequestHttpMethodType.HttpPost,
                Body = content,
                Headers = requestHeaders
            };
        }

        /// <inheritdoc/>
        public override ResponseData ServiceTranslateResponse(HttpResponseMessage response, string langFrom, string langTo)
        {
            YandexResponse yandexResponse = response.Content.ReadFromJsonAsync<YandexResponse>().Result!;

            string result = String.Join("\n", yandexResponse.Text);

            return new ResponseData()
            {
                Text = result,
                From = langFrom,
                To = langTo
            };
        }

        #endregion
    }
}
