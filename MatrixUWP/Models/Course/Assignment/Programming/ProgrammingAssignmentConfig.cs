#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MatrixUWP.Models.Course.Assignment.Programming
{
    class ProgrammingAssignmentConfig
    {
        private List<string>? submission;
        private ProgrammingStandard? standard;

        [JsonProperty("limits")]
        public ProgrammingAssignmentLimits? Limits { get; set; }

        [JsonProperty("grading")]
        public ProgrammingAssignmentGrading? Grading { get; set; }

        [JsonProperty("language")]
        public List<string>? Language { get; set; }

        [JsonProperty("standard")]
        public ProgrammingStandard? Standard
        {
            get => standard;
            set
            {
                standard = value;
                if (standard != null)
                {
                    SupportFileContents?.Clear();
                    SupportFileContents = new List<string>();
                    if (standard.Support is null) return;
                    for (var i = 0; i < standard.Support.Count; i++)
                    {
                        SupportFileContents.Add(standard.Support[i]);
                    }
                }
            }
        }
        [JsonProperty("compilers")]
        public List<string>? Compilers { get; set; }
        [JsonProperty("submission")]
        public List<string>? Submission
        {
            get => submission;
            set
            {
                submission = value;
                if (submission != null)
                {
                    SubmitContents?.Clear();
                    SubmitContents = Enumerable.Repeat("", submission.Count).ToList();
                }
            }
        }
        [JsonProperty("google tests info")]
        public Dictionary<string, string>? GoogleTestsInfo { get; set; }

        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }

        [JsonProperty("standard_language")]
        public string StandardLanguage { get; set; } = "";
        public List<string>? SubmitContents { get; private set; }
        public List<string>? SupportFileContents { get; private set; }
    }
}
