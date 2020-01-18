#nullable enable
﻿namespace MatrixUWP.Views.Parameters.Course
{
    class CourseDetailsParameters : CommonParameters
    {
        public int CourseId { get; set; }
        public CourseDetailsParameters(CommonParameters param) : base(param)
        {
        }
    }
}
