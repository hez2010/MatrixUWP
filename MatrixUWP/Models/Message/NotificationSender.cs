#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MatrixUWP.Models.Message
{
    public class NotificationSender
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public JToken? Name { get; set; }
    }
}
