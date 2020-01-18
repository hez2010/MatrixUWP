using MatrixUWP.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixUWP.Models.Submission
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
    class SubmissionModel
    {
        public static async ValueTask<ResponseModel<List<CourseSubmissionInfoModel>>> FetchCourseSubmissionListAsync(int courseId, int assignmentId) =>
            await App.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments/{assignmentId}/submissions")
                .JsonAsync<ResponseModel<List<CourseSubmissionInfoModel>>>();
    }
}
