#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Submission.Answer
{
    class ChoiceAnswer
    {
        [JsonProperty("question_id")]
        public int QuestionId { get; set; }
        [JsonProperty("choice_id")]
        public List<int>? ChoiceId { get; set; }
    }
}
