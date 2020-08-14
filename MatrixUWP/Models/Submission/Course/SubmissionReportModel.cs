#nullable enable

using Windows.UI;
using Windows.UI.Xaml.Media;

namespace MatrixUWP.Models.Submission.Course
{
    public class SubmissionReportModel
    {
        public string Name { get; set; } = default!;
        public int Grade { get; set; }
        public bool Pass { get; set; }
        public string? Logs { get; set; }
        public bool DisplayLogs => !string.IsNullOrWhiteSpace(Logs);
        public string PassText => Pass ? "通过" : "未通过";
        public Brush PassColor => new SolidColorBrush(Pass ? Colors.MediumSeaGreen : Colors.OrangeRed);
    }
}
