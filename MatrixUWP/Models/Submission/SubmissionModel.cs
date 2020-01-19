#nullable enable
using MatrixUWP.Extensions;
using MatrixUWP.Models.Submission.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixUWP.Models.Submission
{
    class SubmissionModel
    {
        public static async ValueTask<ResponseModel<List<CourseSubmissionInfoModel>>> FetchCourseSubmissionListAsync(int courseId, int assignmentId) =>
            await App.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments/{assignmentId}/submissions")
                .JsonAsync<ResponseModel<List<CourseSubmissionInfoModel>>>();
        public static async ValueTask<ResponseModel<SubmissionDetailsModel<TAnswer, TReport>>> FetchCourseSubmissionAsync<TAnswer, TReport>
            (int courseId, int assignmentId, int submissionId)
            where TAnswer : class
            where TReport : class =>
            await App.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments/{assignmentId}/submissions/{submissionId}")
                .JsonAsync<ResponseModel<SubmissionDetailsModel<TAnswer, TReport>>>();

        public static async ValueTask<ResponseModel> SubmitForCourseAssignment<TAnswer>(int courseId, int assignmentId, TAnswer answer) =>
            await App.MatrixHttpClient.PostJsonAsync($"/api/courses/{courseId}/assignments/{assignmentId}/submissions", answer)
                .JsonAsync<ResponseModel>();
    }
}
