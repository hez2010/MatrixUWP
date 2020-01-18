#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course.Assignment.Programming
{
    class ProgrammingAssignmentConfig
    {

        [JsonProperty("limits")]
        public ProgrammingAssignmentLimits Limits { get; set; } = new ProgrammingAssignmentLimits();

        [JsonProperty("grading")]
        public ProgrammingAssignmentGrading Grading { get; set; } = new ProgrammingAssignmentGrading();

        [JsonProperty("language")]
        public List<string> Language { get; set; } = new List<string>();

        [JsonProperty("standard")]
        public ProgrammingStandard Standard { get; set; } = new ProgrammingStandard();
        [JsonProperty("compilers")]
        public List<string> Compilers { get; set; } = new List<string>();
        [JsonProperty("submission")]
        public List<string> Submission { get; set; } = new List<string>();
        [JsonProperty("google tests info")]
        public Dictionary<string, string> GoogleTestsInfo { get; set; } = new Dictionary<string, string>();

        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }

        [JsonProperty("standard_language")]
        public string StandardLanguage { get; set; } = "";
    }
}
