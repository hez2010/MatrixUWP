#nullable enable
using Newtonsoft.Json;
using System;

namespace MatrixUWP.Models.Message
{
    public class AnnouncementModel : MessageModelBase
    {
        [JsonProperty("escape")]
        public int Escape { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; } = default!;
        [JsonProperty("sn_id")]
        public int AnnouncementId { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        public override string Type { get; set; } = "announcement";
    }
}
