#nullable enable
using MatrixUWP.Models;
using MatrixUWP.Models.Course;
using MatrixUWP.Parameters.Course;
using MatrixUWP.Shared.Models;
using MatrixUWP.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.Course
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Course : Page
    {
        private readonly CourseViewModel viewModel = new CourseViewModel();

        public Course()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            viewModel.Loading = true;

            try
            {
                var coursesResponse = await CourseModel.FetchCourseListAsync();
                if (coursesResponse?.Status == StatusCode.OK) viewModel.Courses = coursesResponse.Data;
                else AppModel.ShowMessage?.Invoke(coursesResponse?.Message ?? "课程列表获取失败");
            }
            catch (Exception ex)
            {
                AppModel.ShowMessage?.Invoke(ex.Message);
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
            }
            finally
            {
                viewModel.Loading = false;
            }
        }

        private void CoursesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.AddedItems.First() is CourseInfoModel course)) return;
            AppModel.NavigateToPage?.Invoke(
                typeof(CourseDetails),
                new CourseDetailsParameters { CourseId = course.CourseId }, -1);
        }
    }
}
