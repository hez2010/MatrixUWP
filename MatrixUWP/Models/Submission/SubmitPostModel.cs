#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.Models.Submission
{
    class SubmitPostModel<TAnswer> where TAnswer : class
    {
        [JsonProperty("detail")]
        public SubmitAnswerModel<TAnswer> Detail { get; set; } = new SubmitAnswerModel<TAnswer>();
    }
}
