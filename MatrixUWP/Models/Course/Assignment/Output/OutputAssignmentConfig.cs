#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course.Assignment.Output
{
    internal class OutputAssignmentConfig
    {
        [JsonProperty("answer_scores")]
        public List<int>? AnswerScores { get; set; }
        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }
        [JsonProperty("standard_answers")]
        public List<string>? StandardAnswers { get; set; }
    }
}
