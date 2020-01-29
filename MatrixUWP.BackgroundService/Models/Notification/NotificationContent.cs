#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.BackgroundService.Models.Notification
{
    class NotificationContent
    {
        [JsonProperty("text")]
        public string Text { get; set; } = "";
    }
}
