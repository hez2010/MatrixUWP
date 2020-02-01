#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.BackgroundService.Models.Notification.Content
{
    class SystemContent
    {
        [JsonProperty("text")]
        public string Text { get; set; } = "";
    }
}
