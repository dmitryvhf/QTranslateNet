using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;

using Microsoft.Net.Http.Headers;

using QTranslateNet.Core;
using QTranslateNet.Core.Helpers;
using QTranslateNet.Core.Infrastructure;
using QTranslateNet.Core.Models;

namespace GrokAITranslateServiceLibrary
{
    /// <summary>
    ///     Сервис перевода текста "Grok Translate"
    /// </summary>
    public class GrokAITranslateService : TranslateServiceBase
    {
        #region TranslateServiceBase implementation

        /// <inheritdoc/>
        protected override String BaseUrlApi => "https://api.mymemory.translated.net";

        /// <inheritdoc/>
        protected override String BaseUrlWeb => "https://mymemory.translated.net";

        /// <inheritdoc/>
        protected override String Name => "GrokAI Translate";

        /// <inheritdoc/>
        protected override String AccessibleName => "GrokAI";

        /// <inheritdoc/>
        protected override String Info => "GrokAI - xAI's Grok" + MyConstants.NLine2 + "https://grok.com/" + MyConstants.NLine2 + "\u00a9 xAI";

        /// <inheritdoc/>
        protected override byte[] ServiceIco => GrokAITranslateResource.ServiceIco;

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
            string requestText = CommonMethods.PrepareSource(text);
            requestText = CommonMethods.LimitSource(requestText);
            requestText = requestText.Trim();
            requestText = CommonMethods.EncodeGetParam(requestText);

            string requestLangFrom = GetSourceLanguageCode(langFrom);
            string requestLangTo = GetTargetLanguageCode(langFrom);

            return BaseUrlWeb +
                $"?q={requestText}&langpair={requestLangFrom}|{requestLangTo}";
        }

        /// <inheritdoc/>
        public override RequestData ServiceTranslateRequest(string text, string langFrom, string langTo)
        {
            // Get body
            string url = $"/get?q={CommonMethods.EncodeGetParam(text)}&langpair={langFrom}|{langTo}";

            // Post body
            Dictionary<string, string> requestHeaders = new Dictionary<string, string>
            {
                { HeaderNames.Accept, "application/json" },
                { HeaderNames.AcceptCharset, "utf-8" },
                { HeaderNames.Connection, "keep-alive" }
            };

            return new RequestData()
            {
                RelativeUrl = url,
                Method = RequestHttpMethodType.HttpGet,
                Headers = requestHeaders
            };
        }

        /// <inheritdoc/>
        public override ResponseData ServiceTranslateResponse(HttpResponseMessage response, string langFrom, string langTo)
        {
            GrokResponse? grokResponse = response.Content.ReadFromJsonAsync<GrokResponse>().Result;

            if (HasApiError(grokResponse))
            {
                return new ResponseData()
                {
                    Text = FormatApiError(grokResponse.ResponseDetails),
                    From = langFrom,
                    To = langTo
                };

            }

            string result = grokResponse.ResponseData.TranslatedText;

            return new ResponseData()
            {
                Text = result,
                From = langFrom,
                To = langTo
            };
        }

        #endregion

        #region Private methods

        private static String GetSourceLanguageCode(String langFrom)
        {
            if (langFrom == MyConstants.AutoDetectLanguage.Code)
            {
                return "autodetect";
            }

            return NormalizeLanguageCode(langFrom);

            //string result = NormalizeLanguageCode(codeFromLanguage(a));
            //return a == UNKNOWN_LANGUAGE_CODE ? SupportedLanguages[ENGLISH_LANGUAGE] : a;
        }

        private static String GetTargetLanguageCode(String langFrom)
        {
            return NormalizeLanguageCode(langFrom);

            //a = normalizeLanguageCode(codeFromLanguage(a));
            //return a == UNKNOWN_LANGUAGE_CODE ? SupportedLanguages[ENGLISH_LANGUAGE] : a;
        }

        private static String NormalizeLanguageCode(String langCode)
        {
            switch (langCode)
            {
                case "he":
                    return "iw";
                case "fil":
                    return "tl";
                default:
                    return langCode;
            }
        }

        private static bool HasApiError([NotNull] GrokResponse? response)
        {
            ArgumentNullException.ThrowIfNull(response);

            return response.ResponseStatus != 200;
        }

        private static String FormatApiError(String? response)
        {
            if (String.IsNullOrWhiteSpace(response))
            {
                return "[No Translation]";
            }

            return response.Substring(0, 120);
        }

        #endregion
    }
}
