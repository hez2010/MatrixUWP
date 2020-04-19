#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.Models.Course.Assignment.Report
{
    internal class ReportAssignmentConfig
    {
        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }
    }
}
