#nullable enable
using MatrixUWP.Shared.Extensions;
using MatrixUWP.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixUWP.Models.Course
{
    public class CourseModel
    {
        public static async ValueTask<ResponseModel<List<CourseInfoModel>>> FetchCourseListAsync()
        {
            return await AppModel.MatrixHttpClient.GetAsync("/api/courses")
.JsonAsync<ResponseModel<List<CourseInfoModel>>>();
        }

        public static async ValueTask<ResponseModel<CourseInfoModel>> FetchCourseAsync(int courseId)
        {
            return await AppModel.MatrixHttpClient.GetAsync($"/api/courses/{courseId}")
.JsonAsync<ResponseModel<CourseInfoModel>>();
        }
    }
}
