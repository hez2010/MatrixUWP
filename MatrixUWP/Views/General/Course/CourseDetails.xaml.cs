using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Parameters;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.General.Course
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CourseDetails : Page
    {
        private readonly CourseDetailsViewModel viewModel = new CourseDetailsViewModel();
        private CourseDetailsParameters? parameters;
        public CourseDetails()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is CourseDetailsParameters p)
            {
                this.parameters = p;
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AdjustElementsSize();
            this.viewModel.Loading = true;
            await Dispatcher.YieldAsync();
            try
            {
                var response = await CourseModel.FetchCourseAsync(this.parameters?.CourseId ?? 0);
                if (response.Status != StatusCode.OK)
                {
                    this.parameters?.ShowMessage(response.Message);
                    return;
                }
                viewModel.Course = response.Data;
            }
            catch (Exception ex)
            {
                this.parameters?.ShowMessage(ex.Message);
            }
            finally
            {
                this.viewModel.Loading = false;
            }
        }

        private void AdjustElementsSize()
        {
            BorderLine.X2 = ActualWidth;
            DescriptionViewer.MaxHeight = Math.Max(ActualHeight - 150, 0);
            DescriptionViewer.MaxWidth = Math.Max(Container.ActualWidth - 16, 0);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) => AdjustElementsSize();
    }
}
