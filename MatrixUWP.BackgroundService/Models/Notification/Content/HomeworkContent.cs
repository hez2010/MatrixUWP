#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.BackgroundService.Models.Notification.Content
{
    class HomeworkContent
    {
        [JsonProperty("action")]
        public string Action { get; set; } = "";
        [JsonProperty("prob_title")]
        public string ProblemTitle { get; set; } = "";
        [JsonProperty("link")]
        public string Link { get; set; } = "";
    }
}
