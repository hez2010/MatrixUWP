#nullable enable
﻿namespace MatrixUWP.Views.Parameters.Course
{
    class CourseAssignmentsParameters : CommonParameters
    {
        public int CourseId { get; set; }
        public CourseAssignmentsParameters(CommonParameters param) : base(param)
        {
        }
    }
}
