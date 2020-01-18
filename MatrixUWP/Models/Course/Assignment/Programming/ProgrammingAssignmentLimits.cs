#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.Models.Course.Assignment.Programming
{
    public class ProgrammingAssignmentLimits
    {
        [JsonProperty("time")]
        public long Time { get; set; }
        [JsonProperty("memory")]
        public long Memory { get; set; }
    }
}
