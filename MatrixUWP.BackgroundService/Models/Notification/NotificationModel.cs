#nullable enable
using Newtonsoft.Json;
using System;

namespace MatrixUWP.BackgroundService.Models.Notification
{
    class NotificationModel
    {
        [JsonProperty("time")]
        public DateTime Time { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; } = "";
        [JsonProperty("sender")]
        public NotificationSender? Sender { get; set; }
        [JsonProperty("content")]
        public NotificationContent? Content { get; set; }
    }
}
