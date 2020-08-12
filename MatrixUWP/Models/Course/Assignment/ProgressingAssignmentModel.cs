#nullable enable
using Newtonsoft.Json;
using System;
using System.Text;

namespace MatrixUWP.Models.Course.Assignment
{
    public class ProgressingAssignmentModel
    {
        [JsonProperty("course_id")]
        public int CourseId { get; set; }

        [JsonProperty("ca_id")]
        public int CourseAssignmentId { get; set; }

        [JsonProperty("asgn_id")]
        public int AssignmentId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = default!;

        [JsonProperty("title")]
        public string Title { get; set; } = default!;

        [JsonProperty("courseName")]
        public string CourseName { get; set; } = default!;

        [JsonProperty("enddate")]
        public DateTime EndDate { get; set; }

        [JsonProperty("grade_at_end")]
        public bool GradeAtEnd { get; set; }

        [JsonProperty("ptype_id")]
        public int PtypeId { get; set; }

        [JsonProperty("remainingTime")]
        public long RemainingTime { get; set; }

        [JsonProperty("grade")]
        public int Grade { get; set; }

        public string RemainingTimeText
        {
            get
            {
                var time = TimeSpan.FromMilliseconds(RemainingTime);
                var sb = new StringBuilder();
                if (time.Days != 0)
                {
                    sb.Append($" {time.Days} 天");
                }
                if (time.Hours != 0)
                {
                    sb.Append($" {time.Hours} 小时");
                }
                sb.Append($" {time.Minutes} 分");
                return sb.ToString().Substring(1);
            }
        }
    }
}
