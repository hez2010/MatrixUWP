using MatrixUWP.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.General.Course
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CourseAssignments : Page
    {
        private readonly CourseAssignmentsViewModel viewModel = new CourseAssignmentsViewModel();
        public CourseAssignments()
        {
            this.InitializeComponent();
        }
    }
}
