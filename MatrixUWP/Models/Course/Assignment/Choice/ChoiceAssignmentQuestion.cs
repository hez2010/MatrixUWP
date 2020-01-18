#nullable enable
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MatrixUWP.Models.Course.Assignment.Choice
{
    public class ChoiceAssignmentQuestion
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("choices")]
        public List<ChoiceAssignmentElement> Choices { get; set; } = new List<ChoiceAssignmentElement>();
        [JsonProperty("grading")]
        public ChoiceAssignmentGrading Grading { get; set; } = new ChoiceAssignmentGrading();
        [JsonProperty("$$hashKey")]
        public string HashKey { get; set; } = "";
        [JsonProperty("caseStatus")]
        public string CaseStatus { get; set; } = "";
        [JsonProperty("choice_type")]
        public string ChoiceType { get; set; } = "";
        [JsonProperty("description")]
        public string Description { get; set; } = "";
        [JsonProperty("explanation")]
        public string Explanation { get; set; } = "";
        [JsonProperty("standard_answer")]
        public List<int> StandardAnswer { get; set; } = new List<int>();
    }
}