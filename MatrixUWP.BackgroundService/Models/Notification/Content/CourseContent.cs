#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.BackgroundService.Models.Notification.Content
{
    class CourseContent
    {
        [JsonProperty("text")]
        public string Text { get; set; } = "";
    }
}
