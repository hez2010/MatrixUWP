#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.BackgroundService.Models.Notification.Content
{
    internal class LibraryContent
    {
        [JsonProperty("action")]
        public string Action { get; set; } = "";
        [JsonProperty("prob_title")]
        public string ProblemTitle { get; set; } = "";
        [JsonProperty("library_name")]
        public string LibraryName { get; set; } = "";
        [JsonProperty("link")]
        public string Link { get; set; } = "";
    }
}
