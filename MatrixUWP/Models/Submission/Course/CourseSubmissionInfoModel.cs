#nullable enable
using Newtonsoft.Json;
using System;

namespace MatrixUWP.Models.Submission.Course
{
    class CourseSubmissionInfoModel
    {
        [JsonProperty("sub_ca_id")]
        public int SubmissionId { get; set; }
        [JsonProperty("sub_asgn_id")]
        public int AssignmentId { get; set; }
        [JsonProperty("submit_at")]
        public DateTime SubmitAt { get; set; }
        [JsonProperty("grade")]
        public int Grade { get; set; }
        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}
