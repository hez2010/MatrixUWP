#nullable enable

using Newtonsoft.Json;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace MatrixUWP.Models.Submission.Programming
{
    public class ProgrammingSubmissionReportStageModel
    {
        [JsonProperty("name")]
        public string Name { get; set; } = default!;
        [JsonProperty("score")]
        public int Score { get; set; }
        [JsonProperty("full_score")]
        public int FullScore { get; set; }
        [JsonProperty("cases")]
        public List<ProgrammingSubmissionReportCaseModel> Cases { get; set; } = new List<ProgrammingSubmissionReportCaseModel>();
        [JsonProperty("status")]
        public string Status { get; set; } = "queued";
        public bool Pass => Status == "accepted";
        public string StatusText => Status switch
        {
            "accepted" => "通过",
            "failed" => "失败",
            "locked" => "未解锁",
            "pending" => "等待中",
            "queued" => "队列中",
            _ => "未知"
        };
        public Brush PassColor => new SolidColorBrush(Pass ? Colors.MediumSeaGreen : Colors.OrangeRed);
    }
}
