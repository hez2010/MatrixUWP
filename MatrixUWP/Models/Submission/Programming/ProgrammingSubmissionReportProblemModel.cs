#nullable enable

using Newtonsoft.Json;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace MatrixUWP.Models.Submission.Programming
{

    public class ProgrammingSubmissionReportProblemModel
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "info";
        [JsonProperty("header")]
        public string Header { get; set; } = default!;
        [JsonProperty("message")]
        public string? Message { get; set; }

        public Brush TypeColor => new SolidColorBrush(Type switch
        {
            "info" => Colors.SteelBlue,
            "warning" => Colors.Orange,
            "error" => Colors.OrangeRed,
            _ => Colors.Gray
        });
    }
}
