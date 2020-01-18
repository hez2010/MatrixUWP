#nullable enable
using Newtonsoft.Json;

namespace MatrixUWP.Models.Course.Assignment.Choice
{
    public class ChoiceAssignmentElement
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("$$hashKey")]
        public string HashKey { get; set; } = "";
        [JsonProperty("textStatus")]
        public int TextStatus { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; } = "";
    }
}