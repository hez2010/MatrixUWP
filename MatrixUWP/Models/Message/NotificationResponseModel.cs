#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Message
{
    public class NotificationResponseModel
    {
        [JsonProperty("notifications")]
        public List<NotificationModel> Notifications { get; set; } = new List<NotificationModel>();
        [JsonProperty("total_num")]
        public int TotalNum { get; set; }
        [JsonProperty("unread_num")]
        public int UnreadNum { get; set; }
    }
}
