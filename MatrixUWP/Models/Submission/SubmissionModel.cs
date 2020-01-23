#nullable enable
using MatrixUWP.Extensions;
using MatrixUWP.Models.Submission.Course;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;

namespace MatrixUWP.Models.Submission
{
    class SubmissionModel
    {
        public static async ValueTask<ResponseModel<List<CourseSubmissionInfoModel>>> FetchCourseSubmissionListAsync(int courseId, int assignmentId)
        {
            return await AppModel.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments/{assignmentId}/submissions")
                .JsonAsync<ResponseModel<List<CourseSubmissionInfoModel>>>();
        }

        public static async ValueTask<ResponseModel<SubmissionDetailsModel<TAnswer, TReport>>> FetchCourseSubmissionAsync<TAnswer, TReport>
           (int courseId, int assignmentId, int submissionId)
           where TAnswer : class
           where TReport : class
        {
            return await AppModel.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments/{assignmentId}/submissions/{submissionId}")
                .JsonAsync<ResponseModel<SubmissionDetailsModel<TAnswer, TReport>>>();
        }

        public static async ValueTask<ResponseModel> SubmitForCourseAssignment<TAnswer>(int courseId, int assignmentId, TAnswer answer)
        {
            return await AppModel.MatrixHttpClient.PostJsonAsync($"/api/courses/{courseId}/assignments/{assignmentId}/submissions", answer)
                .JsonAsync<ResponseModel>();
        }

        public static async ValueTask<ResponseModel> SubmitFileForCourseAssignment(int courseId, int assignmentId, StorageFile file)
        {
            var buffer = await FileIO.ReadBufferAsync(file);
            using var httpContent = new HttpBufferContent(buffer);
            return await AppModel.MatrixHttpClient.PostFileAsync($"/api/courses/{courseId}/assignments/{assignmentId}/submissions", "detail", file)
                .JsonAsync<ResponseModel>();
        }
    }
}
