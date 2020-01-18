using Newtonsoft.Json;

namespace MatrixUWP.Models.Course.Assignment.Choice
{
    public class ChoiceAssignmentGrading
    {
        [JsonProperty("max_grading")]
        public int MaxGrade { get; set; }
        [JsonProperty("half_grading")]
        public int HalfGrading { get; set; }
    }
}