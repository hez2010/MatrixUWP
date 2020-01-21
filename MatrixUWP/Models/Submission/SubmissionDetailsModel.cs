#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.Models.Submission
{
    class SubmissionDetailsModel<TAnswer, TReport>
        where TAnswer : class
        where TReport : class
    {

        [JsonProperty("grade")]
        public int? Grade { get; set; }
        [JsonProperty("answers")]
        public TAnswer? Answers { get; set; }
        [JsonProperty("report")]
        public TReport? Report { get; set; }
        [JsonProperty("sub_ca_id")]
        public int SubmissionId { get; set; }
        [JsonProperty("sub_asgn_id")]
        public int AssignmentId { get; set; }
    }
}
