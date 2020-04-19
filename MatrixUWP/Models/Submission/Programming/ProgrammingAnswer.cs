#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.Models.Submission.Programming
{
    internal class ProgrammingAnswer
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("code")]
        public string? Code { get; set; }
    }
}
