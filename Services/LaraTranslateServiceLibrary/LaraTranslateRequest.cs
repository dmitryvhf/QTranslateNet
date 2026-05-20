using System;
using System.Text.Json.Serialization;

namespace LaraTranslateServiceLibrary
{
    internal sealed class LaraTranslateRequest
    {
        [JsonPropertyName("q")]
        public required String Text { get; set; }

        [JsonPropertyName("source")]
        public required String Source { get; set; }

        [JsonPropertyName("target")]
        public required String Target { get; set; }

        [JsonPropertyName("instructions")]
        public required String[] Instructions { get; set; } = Array.Empty<string>();
    }
}