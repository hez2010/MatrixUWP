#nullable enable
﻿using Newtonsoft.Json;

namespace MatrixUWP.Models.Course
{
    public class AssignmentGrading
    {
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
    }
}
