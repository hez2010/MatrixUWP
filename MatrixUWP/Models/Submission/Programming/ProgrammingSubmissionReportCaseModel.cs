#nullable enable

using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace MatrixUWP.Models.Submission.Programming
{
    public class ProgrammingSubmissionReportCaseModel
    {
        [JsonProperty("name")]
        public string Name { get; set; } = default!;
        [JsonProperty("description")]
        public string? Description { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; } = "queued";
        [JsonProperty("stdin")]
        public string? StdIn { get; set; }
        [JsonProperty("stdout")]
        public string? StdOut { get; set; }
        [JsonProperty("stdout_expect")]
        public string? StdOutExpect { get; set; }
        [JsonProperty("time_used")]
        public long TimeUsed { get; set; }
        [JsonProperty("memory_used")]
        public long MemoryUsed { get; set; }
        [JsonProperty("problems")]
        public List<ProgrammingSubmissionReportProblemModel> Problems { get; set; } = new List<ProgrammingSubmissionReportProblemModel>();
        public string StatusText => Status switch
        {
            "queued" => "队列中",
            "pending" => "等待中",
            "failed" => "失败",
            "accepted" => "通过",
            _ => "未知"
        };
        public bool Pass => Status == "accepted";
        public Brush PassColor => new SolidColorBrush(Pass ? Colors.MediumSeaGreen : Colors.OrangeRed);
    }
}
