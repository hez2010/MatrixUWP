#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course.Assignment.Answer
{
    internal class AnswerAssignmentQuestion
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("grade")]
        public int Grade { get; set; }
        [JsonProperty("keywords")]
        public List<string>? Keywords { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; } = "";
        [JsonProperty("explanation")]
        public string Explanation { get; set; } = "";
        [JsonProperty("standard_answer")]
        public string StandardAnswer { get; set; } = "";
    }
}