using System;
using System.Text.Json.Serialization;

namespace ReversoTranslateServiceLibrary
{
    internal sealed class ReversoTranslateResponse
    {
        [JsonPropertyName("id")]
        public required String Id { get; set; }

        [JsonPropertyName("from")]
        public required String From { get; set; }

        [JsonPropertyName("to")]
        public required String To { get; set; }

        [JsonPropertyName("input")]
        public required String[] Input { get; set; }

        [JsonPropertyName("correctedText")]
        public required String CorrectedText { get; set; }

        [JsonPropertyName("translation")]
        public required String[] Translation { get; set; }
    }

    internal sealed class ReversoTranslateContextResultsResponse
    {
        [JsonPropertyName("results")]
        public required ReversoTranslateContextResultResponse[] ContextResultsResponse { get; set; }

        [JsonPropertyName("transliteration")]
        public required String Transliteration { get; set; }
    }

    internal sealed class ReversoTranslateContextResultResponse
    {
        [JsonPropertyName("translation")]
        public required String Translation { get; set; }

        [JsonPropertyName("sourceExamples")]
        public required String[] SourceExamples { get; set; }

        [JsonPropertyName("targetExamples")]
        public required String[] TargetExamples { get; set; }
    }
}