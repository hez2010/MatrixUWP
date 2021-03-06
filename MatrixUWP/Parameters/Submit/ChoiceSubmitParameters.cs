﻿#nullable enable
using MatrixUWP.Models.Course.Assignment.Choice;
using System.Collections.Generic;
namespace MatrixUWP.Parameters.Submit
{
    internal class ChoiceSubmitParameters
    {
        public string Description { get; set; } = "";
        public string Title { get; set; } = "";
        public int CourseId { get; set; }
        public int AssignmentId { get; set; }
        public List<ChoiceAssignmentQuestion>? Questions { get; set; }
        public int RemainingSubmitTimes { get; set; }
    }
}
