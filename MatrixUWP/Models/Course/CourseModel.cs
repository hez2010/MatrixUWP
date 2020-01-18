#nullable enable
﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixUWP.Extensions;

namespace MatrixUWP.Models.Course
{
    public class CourseModel
    {
        public static async ValueTask<ResponseModel<List<CourseInfoModel>>> FetchCourseListAsync() =>
            await App.MatrixHttpClient.GetAsync("/api/courses")
                .JsonAsync<ResponseModel<List<CourseInfoModel>>>();

        public static async ValueTask<ResponseModel<CourseInfoModel>> FetchCourseAsync(int courseId) =>
            await App.MatrixHttpClient.GetAsync($"/api/courses/{courseId}")
                .JsonAsync<ResponseModel<CourseInfoModel>>();
    }
}
