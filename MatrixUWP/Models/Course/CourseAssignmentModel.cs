#nullable enable
﻿using MatrixUWP.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixUWP.Models.Course
{
    public class CourseAssignmentModel
    {
        public static async ValueTask<ResponseModel<List<CourseAssignmentInfoModel>>> FetchCourseAssignmentListAsync(int courseId) =>
              await App.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments")
                  .JsonAsync<ResponseModel<List<CourseAssignmentInfoModel>>>();

        public static async ValueTask<ResponseModel<CourseAssignmentInfoModel>> FetchCourseAssignmentAsync(int courseId, int courseAssignmentId) =>
            await App.MatrixHttpClient.GetAsync($"/api/courses/{courseId}/assignments/{courseAssignmentId}")
                .JsonAsync<ResponseModel<CourseAssignmentInfoModel>>();
    }
}
