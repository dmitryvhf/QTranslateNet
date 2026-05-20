using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Authentication;

using Microsoft.Net.Http.Headers;

using QTranslateNet.Core;
using QTranslateNet.Core.Helpers;
using QTranslateNet.Core.Infrastructure;
using QTranslateNet.Core.Models;

namespace ReversoTranslateServiceLibrary
{
    /// <summary>
    ///     Сервис перевода текста "Google Translate"
    /// </summary>
    public class ReversoTranslateService : TranslateServiceBase
    {
        #region Private consts

        /// <inheritdoc/>
        protected override String BaseUrlApi => "https://api.reverso.net";

        /// <inheritdoc/>
        protected override String BaseUrlWeb => "https://www.reverso.net";

        /// <inheritdoc/>
        protected override String Name => "Reverso Translate";

        /// <inheritdoc/>
        protected override String AccessibleName => "Reverso";

        /// <inheritdoc/>
        protected override String Info => "Free online translation in French, Spanish, Italian, German, Russian, Portuguese, Hebrew, Japanese, English."
            + MyConstants.NLine2
            + BaseUrlWeb
            + MyConstants.NLine2
            + "© 2021 Reverso-Softissimo";

        /// <inheritdoc/>
        protected override byte[] ServiceIco => ReversoTranslateResource.ServiceIco;

        /// <inheritdoc/>
        protected override Сapability[] Сapabilities => new Сapability[] { Сapability.Translate };

        ///// <inheritdoc/>
        //protected override string[] SupportedLanguages => new string[]
        //{
        //    "-1", "auto", "eng", "rus"
        //};

        #endregion

        #region Constructors

        public ReversoTranslateService()
        {
        }

        #endregion

        #region TranslateServiceBase implementation

        /// <inheritdoc/>
        public override string GetServiceHost(string langFrom, string langTo, string text)
        {
            return BaseUrlApi;
        }

        /// <inheritdoc/>
        public override string GetServiceLink(string langFrom, string langTo, string text)
        {
            return BaseUrlWeb;
        }

        /// <inheritdoc/>
        public override RequestData ServiceTranslateRequest(string text, string langFrom, string langTo)
        {
            // Get body
            string url = "/translate/v1/translation";

            // temp patch
            langFrom = "eng";
            langTo = "rus";

            // Post body
            ReversoTranslateRequest contentBody = new ReversoTranslateRequest()
            {
                From = langFrom,
                To = langTo,
                Input = text
            };

            JsonContent content = JsonContent.Create(contentBody);

            Dictionary<string, string> headers = CommonMethods.HttpDefaultHeaders();
            headers.Add(HeaderNames.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");

            return new RequestData()
            {
                RelativeUrl = url,
                Method = RequestHttpMethodType.HttpPost,
                Body = content,
                Headers = headers,
#pragma warning disable CA5397 // Do not use deprecated SslProtocols values
#pragma warning disable CS0618 // Type or member is obsolete
                SslProtocols = SslProtocols.Default
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore CA5397 // Do not use deprecated SslProtocols values
            };
        }

        /// <inheritdoc/>
        public override ResponseData ServiceTranslateResponse(HttpResponseMessage response, string langFrom, string langTo)
        {
            // string result = response.Content.ReadAsStringAsync().Result;
            ReversoTranslateResponse result = response.Content.ReadFromJsonAsync<ReversoTranslateResponse>().Result!;

            return new ResponseData()
            {
                Text = result.Translation.FirstOrDefault() ?? "[No data returned]",
                From = langFrom,
                To = langTo
            };
        }

        #endregion
    }
}
