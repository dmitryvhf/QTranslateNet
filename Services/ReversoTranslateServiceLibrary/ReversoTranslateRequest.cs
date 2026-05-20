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
        public String Format { get; } = "text";

        [JsonPropertyName("options")]
        public ReversoTranslateOptionsRequest Options { get; } = new ReversoTranslateOptionsRequest();
    }

    internal sealed class ReversoTranslateOptionsRequest
    {
        [JsonPropertyName("origin")]
        public String Origin { get; } = "contextweb";

        [JsonPropertyName("sentenceSplitter")]
        public bool SentenceSplitter { get; } = true;

        [JsonPropertyName("contextResults")]
        public bool ContextResults { get; } = true;
    }
}