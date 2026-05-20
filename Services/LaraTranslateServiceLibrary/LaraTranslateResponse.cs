using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace LaraTranslateServiceLibrary
{
    internal sealed class LaraResponse
    {
        [JsonPropertyName("status")]
        public required int Status { get; set; }

        [JsonPropertyName("content")]
        public required LaraContentResponse Content { get; set; }
    }

    internal sealed class LaraContentResponse
    {
        [JsonPropertyName("translation")]
        public required String Translation { get; set; }
    }
}
