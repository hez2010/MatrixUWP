#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.BackgroundService.Models.Notification.Content
{
    internal class CourseContent
    {
        [JsonProperty("text")]
        public string Text { get; set; } = "";
    }
}
