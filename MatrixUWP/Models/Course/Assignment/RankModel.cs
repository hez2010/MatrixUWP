#nullable enable
using Newtonsoft.Json;
using System;

namespace MatrixUWP.Models.Course.Assignment
{
    public class RankModel
    {
        [JsonProperty("grade")]
        public int Grade { get; set; }
        [JsonProperty("lastSubmissionTime")]
        public DateTime LastSubmissionTime { get; set; }
        [JsonProperty("nickname")]
        public string NickName { get; set; } = default!;
        [JsonProperty("rank")]
        public int Rank { get; set; }
        [JsonProperty("submissionTimes")]
        public int SubmissionTimes { get; set; }
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        public string Description => $"得分: {Grade}, 提交次数: {SubmissionTimes}, 上次提交: {LastSubmissionTime:yyyy/MM/dd HH:mm:ss}";
    }
}
