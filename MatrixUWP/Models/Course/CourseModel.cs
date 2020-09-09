#nullable enable
using MatrixUWP.Models.User;
using MatrixUWP.Shared.Extensions;
using MatrixUWP.Shared.Models;
using MatrixUWP.Shared.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixUWP.Models.Course
{
    public class CourseModel
    {
        public static async ValueTask<ResponseModel<List<CourseInfoModel>>?> FetchCourseListAsync() => await HttpUtils.MatrixHttpClient.GetAsync("/api/courses")
                .JsonAsync<ResponseModel<List<CourseInfoModel>>>();

        public static async ValueTask<ResponseModel<CourseInfoModel>?> FetchCourseAsync(int courseId) => await HttpUtils.MatrixHttpClient.GetAsync($"/api/courses/{courseId}")
                .JsonAsync<ResponseModel<CourseInfoModel>>();
        public static async ValueTask<ResponseModel<List<UserEssentialDataModel>>?> FetchCourseMembersAsync(int courseId) => await HttpUtils.MatrixHttpClient.GetAsync($"api/courses/{courseId}/members")
                .JsonAsync<ResponseModel<List<UserEssentialDataModel>>>();
    }
}
