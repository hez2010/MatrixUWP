#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course.Assignment.Answer
{
    class AnswerAssignmentConfig
    {
        [JsonProperty("questions")]
        public List<AnswerAssignmentQuestion> Questions { get; set; } = new List<AnswerAssignmentQuestion>();
        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }
    }
}
