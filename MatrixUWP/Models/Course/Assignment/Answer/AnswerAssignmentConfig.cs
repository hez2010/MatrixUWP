#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course.Assignment.Answer
{
    internal class AnswerAssignmentConfig
    {
        [JsonProperty("questions")]
        public List<AnswerAssignmentQuestion>? Questions { get; set; }
        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }
    }
}
