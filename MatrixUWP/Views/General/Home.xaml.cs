using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.Utils;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        private HomeParameters parameters = new HomeParameters();

        public Home()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is HomeParameters parameters)
            {
                this.parameters = parameters;
                this.parameters.UserData.Captcha = false;
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

                Debug.WriteLine(result.SerializeJson());
                this.parameters.ShowMessage?.Invoke(result.Message);

                this.parameters.UpdateUserData?.Invoke(result.Data);

                if (!result.Data.Captcha) return;
                var captcha = await UserModel.FetchCaptchaAsync();

                var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);
                await writer.WriteAsync(captcha.Data.Captcha);
                await writer.FlushAsync();
                stream.Position = 0;

                var svg = new SvgImageSource { RasterizePixelWidth = 150, RasterizePixelHeight = 50 };
                await svg.SetSourceAsync(stream.AsRandomAccessStream());
                viewModel.CaptchaData = svg;
            }
            catch (Exception ex)
            {
                this.parameters.ShowMessage?.Invoke(ex.Message);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }

        private async void SignOut_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Loading = true;
            await Dispatcher.Yield();
            try
            {
                var result = await UserModel.SignOutAsync();

                Debug.WriteLine(result.SerializeJson());
                this.parameters.ShowMessage?.Invoke(result.Message);

                this.parameters.UpdateUserData?.Invoke(new UserDataModel());
            }
            catch (Exception ex)
            {
                this.parameters.ShowMessage?.Invoke(ex.Message);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }
    }
}
