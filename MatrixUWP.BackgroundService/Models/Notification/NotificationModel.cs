﻿#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace MatrixUWP.BackgroundService.Models.Notification
{
    internal class NotificationModel
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
        public JObject? Content { get; set; }
    }
}
