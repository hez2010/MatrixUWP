using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Submission.Programming
{
    public class ProgrammingSubmissionReportModel
    {
        [JsonProperty("stages")]
        public List<ProgrammingSubmissionReportStageModel> Stages { get; set; } = new List<ProgrammingSubmissionReportStageModel>();
        [JsonProperty("internal_problems")]
        public List<ProgrammingSubmissionReportProblemModel> InternalProblems { get; set; } = new List<ProgrammingSubmissionReportProblemModel>();
    }
}
