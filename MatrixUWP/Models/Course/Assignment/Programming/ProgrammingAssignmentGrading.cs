#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course.Assignment.Programming
{
    public class ProgrammingAssignmentGrading
    {
        [JsonProperty("style check")]
        public int StyleCheck { get; set; }
        [JsonProperty("google tests")]
        public int GoogleTests { get; set; }
        [JsonProperty("memory check")]
        public int MemoryCheck { get; set; }
        [JsonProperty("random check")]
        public int RandomCheck { get; set; }
        [JsonProperty("static check")]
        public int StaticCheck { get; set; }
        [JsonProperty("compile check")]
        public int CompileCheck { get; set; }
        [JsonProperty("standard check")]
        public int StandardCheck { get; set; }
        [JsonProperty("google tests detail")]
        public Dictionary<string, int> GoogleTestsDetail { get; set; } = new Dictionary<string, int>();
    }
}
