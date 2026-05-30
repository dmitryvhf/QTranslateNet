using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

using Microsoft.Net.Http.Headers;

using QTranslateNet.Core;
using QTranslateNet.Core.Helpers;
using QTranslateNet.Core.Infrastructure;
using QTranslateNet.Core.Models;

namespace MicrosoftTranslateServiceLibrary
{
    /// <summary>
    ///     Сервис перевода текста "Yandex Translate"
    /// </summary>
    public class MicrosoftTranslateService : TranslateServiceBase
    {
        #region Private properties

        private String? OptionsIG;
        private String? OptionsBingKey;
        private String? OptionsBingToken;
        // private String OptionsBingCookie;

        //private const Int32 BING_TRANSLATOR_MAX_SOURCE_LEN = 5000;
        //private const Int32 BING_TRANSLATOR_TOTAL_SOURCE_LEN = 2000;
        //private const Int32 BING_TRANSLATOR_MAX_CHUNK_LEN = 900;

        private readonly Regex _regex1 = new Regex("IG:\"(\\S{32})\"",
                RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        private readonly Regex _regex2 = new Regex("var\\sparams_AbusePreventionHelper\\s?=\\s?\\[(\\d+),\"(\\S*)\",\\d+\\]",
            RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region TranslateServiceBase implementation

        /// <inheritdoc/>
        protected override String BaseUrlApi => "https://www.bing.com";

        /// <inheritdoc/>
        protected override String BaseUrlWeb => "https://www.bing.com/translator";
        /// <inheritdoc/>
        protected override String Name => "Bing Translator";

        /// <inheritdoc/>
        protected override String AccessibleName => "Bing";

        /// <inheritdoc/>
        protected override String Info => "Microsoft Bing Translator web service."
            + MyConstants.NLine2 + BaseUrlWeb + MyConstants.NLine2
            + "\u00a9 Microsoft";

        /// <inheritdoc/>
        protected override byte[] ServiceIco => MicrosoftTranslateResource.ServiceIco;

        /// <inheritdoc/>
        protected override Capability[] Capabilities => new Capability[] { Capability.Translate };

        /// <inheritdoc/>
        public override String GetServiceHost(string langFrom, string langTo, string text)
        {
            return BaseUrlApi;
        }

        /// <inheritdoc/>
        public override String GetServiceLink(string langFrom, string langTo, string text)
        {
            return BaseUrlWeb + $"?text={text}&from={langFrom}&to={langTo}";
        }

        /// <inheritdoc/>
        public override bool ServiceTranslateBootstrapRequest(
            String text,
            String langFrom,
            String langTo,
            [NotNullWhen(true)] out RequestData? requestData)
        {
            // Get body
            string url = $"/translator";

            Dictionary<string, string> requestHeaders = CommonMethods.HttpDefaultHeaders();

            _ = requestHeaders.Remove(HeaderNames.AcceptEncoding);
            requestHeaders.Add(HeaderNames.AcceptEncoding, "identity");
            requestHeaders.Add(HeaderNames.Referer, BaseUrlWeb);
            requestHeaders.Add(HeaderNames.Connection, "keep-alive");

            requestData = new RequestData()
            {
                RelativeUrl = url,
                Method = RequestHttpMethodType.HttpGet,
                Headers = requestHeaders
            };

            return true;
        }

        /// <inheritdoc/>
        public override bool ServiceTranslateBootstrapResponse(
            HttpResponseMessage response,
            String langFrom,
            String langTo)
        {
            string html = response.Content.ReadAsStringAsync().Result;

            // Values 1
            Match ss1 = _regex1.Match(html);
            if (!ss1.Success)
            {
                return false;
            }

            OptionsIG = ss1.Groups[1].Value;

            // Values 2
            Match ss2 = _regex2.Match(html);
            if (!ss2.Success)
            {
                return false;
            }

            OptionsBingKey = ss2.Groups[1].Value;
            OptionsBingToken = ss2.Groups[2].Value;

            return true;
        }

        /// <inheritdoc/>
        public override RequestData[] ServiceTranslateRequest(string text, string langFrom, string langTo)
        {
            if (String.IsNullOrWhiteSpace(OptionsIG)
                || String.IsNullOrWhiteSpace(OptionsBingToken)
                || String.IsNullOrWhiteSpace(OptionsBingKey))
            {
                throw new ArgumentException("Сервис не готов к работе");
            }

            // Get body
            string url = $"/ttranslatev3?isVertical=1&&IG=" + OptionsIG + "&IID=translator.5027";

            string preparedText = CommonMethods.LimitSource(text);

            // Post body
            HttpContent content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["text"] = CommonMethods.EncodePostParam(preparedText),
                ["fromLang"] = langFrom,
                ["to"] = langTo,
                ["token"] = OptionsBingToken,
                ["key"] = OptionsBingKey,
                ["tryFetchingGenderDebiasedTranslations"] = "true"
            });

            Dictionary<string, string> requestHeaders = CommonMethods.HttpDefaultHeaders();

            _ = requestHeaders.Remove(HeaderNames.AcceptEncoding);
            requestHeaders.Add(HeaderNames.AcceptEncoding, "identity");
            requestHeaders.Add(HeaderNames.Origin, BaseUrlApi);
            requestHeaders.Add(HeaderNames.Referer, BaseUrlWeb);
            requestHeaders.Add(HeaderNames.XRequestedWith, "XMLHttpRequest");
            requestHeaders.Add(HeaderNames.Connection, "keep-alive");

            return new RequestData[]
            {
                new RequestData()
                {
                RelativeUrl = url,
                Method = RequestHttpMethodType.HttpPost,
                Body = content,
                Headers = requestHeaders
                }
            };
        }

        /// <inheritdoc/>
        public override ResponseData ServiceTranslateResponse(HttpResponseMessage[] responses, string langFrom, string langTo)
        {
            string result = responses[0].Content.ReadAsStringAsync().Result;
            if (result.Contains("\"statusCode\""))
            {
                // {"statusCode":205,"errorMessage":""}
                return new ResponseData()
                {
                    Text = MyConstants.NoDataReturnedMessage,
                    From = langFrom,
                    To = langTo
                };
            }

            MicrosoftTranslateResponse[] microsoftResponse = responses[0].Content.ReadFromJsonAsync<MicrosoftTranslateResponse[]>().Result!;
            if (microsoftResponse.Length == 0)
            {
                return new ResponseData()
                {
                    Text = MyConstants.NoDataReturnedMessage,
                    From = langFrom,
                    To = langTo
                };
            }

            return new ResponseData()
            {
                Text = microsoftResponse.First().Translations.First().Text,
                From = langFrom,
                To = langTo
            };
        }

        #endregion
    }
}
