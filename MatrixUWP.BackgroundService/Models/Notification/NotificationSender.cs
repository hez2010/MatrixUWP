﻿#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.BackgroundService.Models.Notification
{
    internal class NotificationSender
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } = "";
    }
}
