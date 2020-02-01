#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.BackgroundService.Models.Notification.Content
{
    class DiscussionContent
    {
        [JsonProperty("title")]
        public string Title { get; set; } = "";
        [JsonProperty("rep_id")]
        public int? ReplyId { get; set; }
        [JsonProperty("text")]
        public string? Text { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; } = "";
    }
}
