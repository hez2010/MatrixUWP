#nullable enable
using MatrixUWP.Models.Course.Assignment.Choice;
using System.Collections.Generic;
namespace MatrixUWP.Views.Parameters.Submit
{
    class ChoiceSubmitParameters : CommonParameters
    {
        public ChoiceSubmitParameters(CommonParameters param) : base(param)
        {
        }

        public string Description { get; set; } = "";
        public string Title { get; set; } = "";
        public int CourseId { get; set; }
        public int AssignmentId { get; set; }
        public List<ChoiceAssignmentQuestion> Questions { get; set; } = new List<ChoiceAssignmentQuestion>();
    }
}
