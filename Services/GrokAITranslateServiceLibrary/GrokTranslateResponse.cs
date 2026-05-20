using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GrokAITranslateServiceLibrary
{
    internal sealed class GrokResponse
    {
        [JsonPropertyName("responseData")]
        public required GrokDataResponse ResponseData { get; set; }

        [JsonPropertyName("responseStatus")]
        public required int ResponseStatus { get; set; }

        [JsonPropertyName("responseDetails")]
        public required String ResponseDetails { get; set; }
    }

    internal sealed class GrokDataResponse
    {
        [JsonPropertyName("translatedText")]
        public required String TranslatedText { get; set; }

        //[JsonPropertyName("match")]
        //public Int32 Match { get; set; }
    }
}
