#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course.Assignment.Output
{
    class OutputAssignmentConfig
    {
        [JsonProperty("answer_scores")]
        public List<int> AnswerScores { get; set; } = new List<int>();
        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }
        [JsonProperty("standard_answers")]
        public List<int> StandardAnswers { get; set; } = new List<int>();
    }
}
