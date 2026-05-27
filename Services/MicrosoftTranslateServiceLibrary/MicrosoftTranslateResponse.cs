using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MicrosoftTranslateServiceLibrary
{
    internal sealed class MicrosoftTranslateResponse
    {
        [JsonPropertyName("usedLLM")]
        public required bool UsedLLM { get; set; }

        [JsonPropertyName("translations")]
        public required MicrosoftTranslateTranslationsResponse[] Translations { get; set; }
    }

    internal sealed class MicrosoftTranslateTranslationsResponse
    {
        [JsonPropertyName("text")]
        public required String Text { get; set; }

        [JsonPropertyName("to")]
        public required String To { get; set; }

        [JsonPropertyName("Transliteration")]
        public MicrosoftTranslateTransliterationResponse? Transliteration { get; set; }
    }

    internal sealed class MicrosoftTranslateTransliterationResponse
    {
        [JsonPropertyName("text")]
        public required String Text { get; set; }

        [JsonPropertyName("script")]
        public required String Script { get; set; }
    }
}