#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course.Assignment.Choice
{
    class ChoiceAssignmentConfig
    {
        [JsonProperty("questions")]
        public List<ChoiceAssignmentQuestion> Questions { get; set; } = new List<ChoiceAssignmentQuestion>();
    }
}
