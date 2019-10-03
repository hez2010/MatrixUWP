using System;
using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MatrixUWP.Views.Parameters;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.General
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Course : Page
    {
        private readonly CourseViewModel viewModel = new CourseViewModel();
        private CourseParameters? parameters;

        public Course()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is CourseParameters param)
            {
                this.parameters = param;
                this.parameters.UserData.Captcha = false;
            }
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            viewModel.Loading = true;
            await Dispatcher.Yield();

            try
            {
                var coursesResponse = await CourseModel.FetchCourseListAsync();
                if (coursesResponse.Status == "OK") viewModel.Courses = coursesResponse.Data;
                else this.parameters?.ShowMessage(coursesResponse.Message);
            }
            catch (Exception ex)
            {
                this.parameters?.ShowMessage(ex.Message);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }
    }
}
