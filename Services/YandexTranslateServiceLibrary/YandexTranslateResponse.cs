using System;
using System.Text.Json.Serialization;

namespace YandexTranslateServiceLibrary
{
    internal sealed class YandexResponse
    {
        [JsonPropertyName("code")]
        public required int Code { get; set; }

        [JsonPropertyName("text")]
        public required String[] Text { get; set; }
    }
}
