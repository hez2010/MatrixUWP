#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.User
{
    public class UserAddition
    {
        [JsonProperty("type")]
        public int? Type { get; set; }
        [JsonProperty("fresh")]
        public int? Fresh { get; set; }
        [JsonProperty("grade")]
        public Dictionary<string, int>? Grade { get; set; }
        [JsonProperty("major")]
        public string? Major { get; set; }
        [JsonProperty("school")]
        public string? School { get; set; }
        [JsonProperty("bkmajor")]
        public string? Bkmajor { get; set; }
        [JsonProperty("insider")]
        public InsiderModel? Insider { get; set; }
        [JsonProperty("language")]
        public string? Language { get; set; }
        [JsonProperty("editor_config")]
        public EditorConfig? EditorConfig { get; set; }
    }
}
