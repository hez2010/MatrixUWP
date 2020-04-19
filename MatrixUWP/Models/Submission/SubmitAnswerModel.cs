#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.Models.Submission
{
    internal class SubmitAnswerModel<TAnswer> where TAnswer : class
    {
        [JsonProperty("answers")]
        public TAnswer? Answers { get; set; }
    }
}
