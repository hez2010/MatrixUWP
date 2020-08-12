#nullable enable
using MatrixUWP.Shared.Extensions;
using MatrixUWP.Shared.Models;
using MatrixUWP.Shared.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixUWP.Models.Course.Assignment
{
    public class CourseAssignmentModel
    {
        public static async ValueTask<ResponseModel<List<CourseAssignmentDetailsModel>>?> FetchCourseAssignmentListAsync(int courseId) => await HttpUtils.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments")
                .JsonAsync<ResponseModel<List<CourseAssignmentDetailsModel>>>();

        public static async ValueTask<ResponseModel<CourseAssignmentDetailsModel>?> FetchCourseAssignmentAsync(int courseId, int courseAssignmentId) => await HttpUtils.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments/{courseAssignmentId}")
                .JsonAsync<ResponseModel<CourseAssignmentDetailsModel>>();

        public static async ValueTask<ResponseModel<List<ProgressingAssignmentModel>>?> FetchProgressingAssignmentListAsync() => await HttpUtils.MatrixHttpClient.GetAsync("/api/courses/assignments?state=progressing")
                .JsonAsync<ResponseModel<List<ProgressingAssignmentModel>>>();
    }
}
