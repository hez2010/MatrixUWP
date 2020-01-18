#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.Models.Course.Assignment.File
{
    class FileAssignmentConfig
    {
        [JsonProperty("filename")]
        public string FileName { get; set; } = "";
        [JsonProperty("standard_score")]
        public int StandardScore { get; set; }
    }
}
