#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course.Assignment.Programming
{
    public class ProgrammingStandard
    {
        [JsonProperty("support")]
        public List<string> Support { get; set; } = new List<string>();
        [JsonProperty("standard_input")]
        public List<string> StandardInput { get; set; } = new List<string>();
        [JsonProperty("standard_output")]
        public List<string> StandardOutput { get; set; } = new List<string>();
        [JsonProperty("rwfiles")]
        public List<string> RwFiles { get; set; } = new List<string>();
    }
}