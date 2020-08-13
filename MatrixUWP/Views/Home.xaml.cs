#nullable enable
using MatrixUWP.Models;
using MatrixUWP.Models.Course.Assignment;
using MatrixUWP.Models.User;
using MatrixUWP.Parameters.Course;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Course;
using MatrixUWP.Parameters.Submit;
using MatrixUWP.Views.Submit;
using System;
using System.Diagnostics;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        private readonly HomeViewModel viewModel = new HomeViewModel();

        public Home()
        {
            InitializeComponent();
        }

        private async void SignIn_Click(object sender, RoutedEventArgs? e)
        {
            viewModel.Signining = true;

            try
            {
                var result = await (string.IsNullOrEmpty(viewModel.Captcha) ? UserModel.SignInAsync(viewModel.UserName, viewModel.Password)
                    : UserModel.SignInAsync(viewModel.UserName, viewModel.Password, viewModel.Captcha));

                if (result?.Data == null) throw new InvalidOperationException("Network Error");

                AppModel.ShowMessage?.Invoke(result.Message);
                viewModel.CaptchaNeeded = result.Data.Captcha;
                if (!result.Data.Captcha) return;
                var captcha = await UserModel.FetchCaptchaAsync();

                var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);
                await writer.WriteAsync(captcha?.Data?.Captcha ?? "");
                await writer.FlushAsync();
                stream.Position = 0;

                var svg = new SvgImageSource { RasterizePixelWidth = 150, RasterizePixelHeight = 50 };
                await svg.SetSourceAsync(stream.AsRandomAccessStream());
                viewModel.CaptchaData = svg;
            }
            catch (Exception ex)
            {
                AppModel.AppConfiguration.SavedUserName = "";
                AppModel.AppConfiguration.SavedPassword = "";
                AppModel.ShowMessage?.Invoke(ex.Message);
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
            }
            finally
            {
                viewModel.Signining = false;
            }
        }

        private void Login_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key != Windows.System.VirtualKey.Enter) return;
            if (!viewModel.SignInButtonEnabled) return;
            SignIn_Click(this, null);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!UserModel.CurrentUser.SignedIn)
            {
                viewModel.UserName = AppModel.AppConfiguration.SavedUserName;
                viewModel.Password = AppModel.AppConfiguration.SavedPassword;
            }
            else
            {
                LoadProgressingAssignments();
            }
            UserModel.OnUserDataUpdate += LoadProgressingAssignments;
        }

        private async void LoadProgressingAssignments()
        {
            if (!UserModel.CurrentUser.SignedIn) return; 
            viewModel.Signining = true;
            var response = await CourseAssignmentModel.FetchProgressingAssignmentListAsync();
            if (response?.Status == Shared.Models.StatusCode.OK)
            {
                viewModel.ProgressingAssignments = response.Data;
            }
            viewModel.Signining = false;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            UserModel.OnUserDataUpdate -= LoadProgressingAssignments;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(sender is ListView lv)) return;
            if (!(lv.SelectedItem is ProgressingAssignmentModel assignment)) return;

            if (assignment.Type.EndsWith("Programming"))
            {
                AppModel.NavigateToPage?.Invoke(typeof(CourseAssignments), new CourseAssignmentsParameters
                {
                    CourseId = assignment.CourseId,
                    Title = assignment.CourseName,
                    JumpAssignmentId = assignment.CourseAssignmentId
                }, 1);
            }
        }
    }
}
