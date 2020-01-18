#nullable enable
﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course
{
    public class AssignmentConfig
    {
        [JsonProperty("limits")]
        public AssignmentLimits Limits { get; set; } = new AssignmentLimits();

        [JsonProperty("grading")]
        public AssignmentGrading Grading { get; set; } = new AssignmentGrading();

        [JsonProperty("language")]
        public List<string> Language { get; set; } = new List<string>();

        // TODO： other properties

        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }

        [JsonProperty("standard_language")]
        public string StandardLanguage { get; set; } = "";
    }
}
