using System;
using System.Linq;
using System.Net.Http;

using QTranslateNet.Core;
using QTranslateNet.Core.Helpers;
using QTranslateNet.Core.Infrastructure;
using QTranslateNet.Core.Models;

namespace GoogleTranslateServiceLibrary
{
    /// <summary>
    ///     Сервис перевода текста "Google Translate"
    /// </summary>
    public class GoogleTranslateService : TranslateServiceBase
    {
        #region TranslateServiceBase implementation

        /// <inheritdoc/>
        protected override String BaseUrlApi => "https://translate.googleapis.com";

        /// <inheritdoc/>
        protected override String BaseUrlWeb => "https://translate.google.com";

        /// <inheritdoc/>
        protected override String Name => "Google Translate";

        /// <inheritdoc/>
        protected override String AccessibleName => "Google";

        /// <inheritdoc/>
        protected override String Info => "Google's free online language translation service instantly translates text and web pages."
            + MyConstants.NLine2
            + BaseUrlApi
            + MyConstants.NLine2
            + "\u00a9 2020 Google";

        /// <inheritdoc/>
        protected override byte[] ServiceIco => GoogleTranslateResource.ServiceIco;

        /// <inheritdoc/>
        protected override Capability[] Capabilities => new Capability[] { Capability.Translate, Capability.DetectLanguage };

        //protected override string[] SupportedLanguages
        //{
        //    get
        //    {
        //        return base.SupportedLanguages
        //            .Where(x => x == "en" || x == "ru")
        //            .ToArray();
        //    }
        //}

        /// <inheritdoc/>
        public override string GetServiceHost(string langFrom, string langTo, string text)
        {
            return BaseUrlApi;
        }

        /// <inheritdoc/>
        public override string GetServiceLink(string langFrom, string langTo, string text)
        {
            return BaseUrlWeb + "/" + $"#{langFrom}/{langTo}/{CommonMethods.EncodeGetParam(text)}";
        }

        /// <inheritdoc/>
        public override RequestData[] ServiceTranslateRequest(string text, string langFrom, string langTo)
        {
            string url = $"/translate_a/single?client=gtx&sl={langFrom}&tl={langTo}&dt=t&q={CommonMethods.EncodeGetParam(text)}";

            return new RequestData[]
            {
                new RequestData()
                {
                    RelativeUrl = url,
                    Method = RequestHttpMethodType.HttpGet
                }
            };
        }

        /// <inheritdoc/>
        public override ResponseData ServiceTranslateResponse(HttpResponseMessage[] responses, string langFrom, string langTo)
        {
            string result = responses[0].Content.ReadAsStringAsync().Result;

            // todo json parse?
            result = result.Substring(4, result.IndexOf('"', 4) - 4);

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
