using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.Utils;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Parameters;
using System;
using System.Diagnostics;
using System.IO;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatrixUWP.Views.General
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        private readonly HomeViewModel viewModel = new HomeViewModel();
        private HomeParameters? parameters;

        public Home()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is HomeParameters param)
            {
                this.parameters = param;
                this.parameters.UserData.Captcha = false;
            }
            if (!(parameters?.UserData?.SignedIn ?? false))
            {
                viewModel.UserName = App.AppConfiguration.SavedUserName;
                viewModel.Password = App.AppConfiguration.SavedPassword;
                if (!string.IsNullOrEmpty(viewModel.UserName) && !string.IsNullOrEmpty(viewModel.Password))
                {
                    SignIn_Click(this, null);
                }
            }
        }

        private async void SignIn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Loading = true;
            await Dispatcher.Yield();
            try
            {
                var result = await (string.IsNullOrEmpty(viewModel.Captcha) ? UserModel.SignInAsync(viewModel.UserName, viewModel.Password)
                : UserModel.SignInAsync(viewModel.UserName, viewModel.Password, viewModel.Captcha));

                if (result?.Data == null) throw new InvalidOperationException("Network Error");

                this.parameters?.ShowMessage?.Invoke(result.Message);
                this.parameters?.UpdateUserData?.Invoke(result.Data);

                if (!result.Data.Captcha) return;
                var captcha = await UserModel.FetchCaptchaAsync();

                var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);
                await writer.WriteAsync(captcha.Data?.Captcha);
                await writer.FlushAsync();
                stream.Position = 0;

                var svg = new SvgImageSource { RasterizePixelWidth = 150, RasterizePixelHeight = 50 };
                await svg.SetSourceAsync(stream.AsRandomAccessStream());
                viewModel.CaptchaData = svg;
            }
            catch (Exception ex)
            {
                App.AppConfiguration.SavedUserName = "";
                App.AppConfiguration.SavedPassword = "";
                this.parameters?.ShowMessage?.Invoke(ex.Message);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }

        private void Editor_TextChanging(RichEditBox sender, RichEditBoxTextChangingEventArgs args)
        {
            var position = sender.Document.Selection.StartPosition;
            var range = sender.Document.GetRange(Math.Max(0, position - 10), position);
            range.CharacterFormat.ForegroundColor = Colors.Red;
        }
    }
}
