#nullable enable
using MatrixUWP.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixUWP.Models.Course.Assignment
{
    public class CourseAssignmentModel
    {
        public static async ValueTask<ResponseModel<List<CourseAssignmentDetailsModel>>> FetchCourseAssignmentListAsync(int courseId)
        {
            return await AppModel.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments")
                .JsonAsync<ResponseModel<List<CourseAssignmentDetailsModel>>>();
        }

        public static async ValueTask<ResponseModel<CourseAssignmentDetailsModel>> FetchCourseAssignmentAsync(int courseId, int courseAssignmentId)
        {
            return await AppModel.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments/{courseAssignmentId}")
                .JsonAsync<ResponseModel<CourseAssignmentDetailsModel>>();
        }
    }
}
