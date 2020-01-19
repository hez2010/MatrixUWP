#nullable enable
using MatrixUWP.ViewModels;
using System;
using System.Collections.Generic;

namespace MatrixUWP.Views.Parameters.Submit
{
    class ProgrammingSubmitParameters : CommonParameters
    {
        public ProgrammingSubmitParameters(CommonParameters param) : base(param)
        {
        }

        public string Description { get; set; } = "";
        public string Title { get; set; } = "";
        public int CourseId { get; set; }
        public int AssignmentId { get; set; }
        public List<string>? Submissions { get; set; }
        public List<string>? Supports { get; set; }
        public List<string>? Languages { get; set; }
        public List<string>? SubmitContents { get; set; }
        public Func<int, string?>? GetContent { get; set; }
        public Action<int, string>? SetContent { get; set; }
    }
}
