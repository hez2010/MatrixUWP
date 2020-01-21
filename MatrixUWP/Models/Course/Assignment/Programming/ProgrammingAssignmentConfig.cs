#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course.Assignment.Programming
{
    class ProgrammingAssignmentConfig
    {
        [JsonProperty("limits")]
        public ProgrammingAssignmentLimits? Limits { get; set; }

        [JsonProperty("grading")]
        public ProgrammingAssignmentGrading? Grading { get; set; }

        [JsonProperty("language")]
        public List<string>? Language { get; set; }

        [JsonProperty("standard")]
        public ProgrammingStandard? Standard { get; set; }
        [JsonProperty("compilers")]
        public List<string>? Compilers { get; set; }
        [JsonProperty("submission")]
        public List<string>? Submission { get; set; }
        [JsonProperty("google tests info")]
        public Dictionary<string, string>? GoogleTestsInfo { get; set; }

        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }

        [JsonProperty("standard_language")]
        public string StandardLanguage { get; set; } = "";
        public Dictionary<string, string> SubmitContents { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, string> SupportContents { get; private set; } = new Dictionary<string, string>();
    }
}
