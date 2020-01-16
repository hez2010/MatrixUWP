using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Parameters.Course;
using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.General.Course
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CourseAssignments : Page
    {
        private readonly CourseAssignmentsViewModel viewModel = new CourseAssignmentsViewModel();
        private CourseAssignmentsParameters? parameters;
        public CourseAssignments()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            parameters = e.Parameter as CourseAssignmentsParameters;
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            viewModel.Loading = true;
            await Dispatcher.YieldAsync();

            try
            {
                var response = await CourseAssignmentModel.FetchCourseAssignmentListAsync(parameters?.CourseId ?? 0);
                if (response.Status != StatusCode.OK)
                {
                    parameters?.ShowMessage(response.Message);
                    return;
                }
                viewModel.Assignments = response.Data;
            }
            catch (Exception ex)
            {
                parameters?.ShowMessage(ex.Message);
                Debug.Fail(ex.Message, ex.StackTrace);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }
    }
}
