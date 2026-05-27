using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

using Microsoft.Net.Http.Headers;

using QTranslateNet.Core;
using QTranslateNet.Core.Helpers;
using QTranslateNet.Core.Infrastructure;
using QTranslateNet.Core.Models;

namespace LaraTranslateServiceLibrary
{
    /// <summary>
    ///     Сервис перевода текста "Lara Translate"
    /// </summary>
    public class LaraTranslateService : TranslateServiceBase
    {
        #region TranslateServiceBase implementation

        /// <inheritdoc/>
        protected override String BaseUrlApi => "https://webapi.laratranslate.com";

        /// <inheritdoc/>
        protected override String BaseUrlWeb => "https://laratranslate.com";

        /// <inheritdoc/>
        protected override String Name => "Lara Translate";

        /// <inheritdoc/>
        protected override String AccessibleName => "Lara";

        /// <inheritdoc/>
        protected override String Info => "Lara Translate: автоматический перевод с поддержкой автоопределения языка.";

        /// <inheritdoc/>
        protected override byte[] ServiceIco => LaraTranslateResource.ServiceIco;

        /// <inheritdoc/>
        protected override Capability[] Capabilities => new Capability[] { Capability.Translate };

        /// <inheritdoc/>
        public override string GetServiceHost(string langFrom, string langTo, string text)
        {
            return BaseUrlApi;
        }

        /// <inheritdoc/>
        public override string GetServiceLink(string langFrom, string langTo, string text)
        {
            String requestText = CommonMethods.EncodeGetParam(text);

            return BaseUrlWeb +
                $"/?text={requestText}&sl={langFrom}&tl={langTo}";
        }

        /// <inheritdoc/>
        public override RequestData[] ServiceTranslateRequest(string text, string langFrom, string langTo)
        {
            // Get body
            String requestText = CommonMethods.EncodeGetParam(text);
            string relativeUrl = $"/translate?text={requestText}&sl={langFrom}&tl={langTo}";

            // Post body
            LaraTranslateRequest contentBody = new LaraTranslateRequest()
            {
                Text = text,
                Source = langFrom,
                Target = langTo,
                Instructions = Array.Empty<string>()
            };

            HttpContent content = JsonContent.Create(contentBody);

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { HeaderNames.Accept, "application/json" },
                { HeaderNames.Origin, "https://laratranslate.com" },
                { HeaderNames.Referer, "https://laratranslate.com" }
            };

            return new RequestData[]
            {
                new RequestData()
                {
                    RelativeUrl = relativeUrl,
                    Method = RequestHttpMethodType.HttpPost,
                    Body = content,
                    Headers = headers
                }
            };
        }

        /// <inheritdoc/>
        public override ResponseData ServiceTranslateResponse(HttpResponseMessage[] responses, string langFrom, string langTo)
        {
            LaraResponse laraResponse = responses[0].Content.ReadFromJsonAsync<LaraResponse>().Result!;

            if (laraResponse.Status != 200)
            {
                return new ResponseData()
                {
                    Text = "[ERR] Code: " + laraResponse.Status,
                    From = langFrom,
                    To = langTo
                };
            }

            string result = laraResponse.Content.Translation;

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
