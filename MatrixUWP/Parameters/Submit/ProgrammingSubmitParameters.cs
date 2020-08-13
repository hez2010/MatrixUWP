#nullable enable
using System;
using System.Collections.Generic;

namespace MatrixUWP.Parameters.Submit
{
    internal class ProgrammingSubmitParameters
    {
        public string Description { get; set; } = "";
        public string Title { get; set; } = "";
        public int CourseId { get; set; }
        public int AssignmentId { get; set; }
        public List<string>? Submissions { get; set; }
        public List<string>? Supports { get; set; }
        public Func<string, bool, string>? GetContent { get; set; }
        public Action<string, string>? SetContent { get; set; }
        public int RemainingSubmitTimes { get; set; }
    }
}
