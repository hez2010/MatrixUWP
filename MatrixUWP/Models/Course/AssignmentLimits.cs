#nullable enable
﻿using Newtonsoft.Json;

namespace MatrixUWP.Models.Course
{
    public class AssignmentLimits
    {
        [JsonProperty("time")]
        public long Time { get; set; }
        [JsonProperty("memory")]
        public long Memory { get; set; }
    }
}
