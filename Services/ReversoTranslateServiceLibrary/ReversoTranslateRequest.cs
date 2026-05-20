using System;
using System.Text.Json.Serialization;

namespace ReversoTranslateServiceLibrary
{
    internal sealed class ReversoTranslateRequest
    {
        [JsonPropertyName("from")]
        public required String From { get; set; }

        [JsonPropertyName("to")]
        public required String To { get; set; }

        [JsonPropertyName("input")]
        public required String Input { get; set; }

        [JsonPropertyName("format")]
        public String Format => "text";

        [JsonPropertyName("options")]
        public ReversoTranslateOptionsRequest Options => new ReversoTranslateOptionsRequest();
    }

    internal sealed class ReversoTranslateOptionsRequest
    {
        [JsonPropertyName("origin")]
        public String Origin => "contextweb";

        [JsonPropertyName("sentenceSplitter")]
        public bool SentenceSplitter => true;

        [JsonPropertyName("contextResults")]
        public bool ContextResults => true;
    }
}