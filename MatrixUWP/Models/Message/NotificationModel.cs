#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace MatrixUWP.Models.Message
{
    public class NotificationModel : MessageModelBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("time")]
        public DateTime Time { get; set; }
        [JsonProperty("type")]
        public override string Type { get; set; } = default!;
        [JsonProperty("sender")]
        public NotificationSender Sender { get; set; } = default!;
        [JsonProperty("content")]
        public JToken Content { get; set; } = default!;
    }
}
