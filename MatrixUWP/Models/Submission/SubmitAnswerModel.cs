#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.Models.Submission
{
    class SubmitAnswerModel<TAnswer> where TAnswer : class
    {
        [JsonProperty("answers")]
        public TAnswer? Answers { get; set; }
    }
}
