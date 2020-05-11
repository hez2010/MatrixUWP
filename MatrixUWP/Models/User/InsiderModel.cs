#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.Models.User
{
    public class InsiderModel
    {
        [JsonProperty("joined")]
        public bool? Joined { get; set; }
    }
}