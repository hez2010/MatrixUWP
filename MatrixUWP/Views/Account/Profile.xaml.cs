using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.Utils;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Parameters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MatrixUWP.Views.Account
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Profile : Page
    {
        private readonly ProfileViewModel viewModel = new ProfileViewModel();
        private ProfileParameters? parameters;
        public Profile()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ProfileParameters p)
            {
                this.parameters = p;
                this.viewModel.UserData = p.UserData;
            }
        }

        private async void SignOut_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Loading = true;
            await Dispatcher.Yield();
            try
            {
                var result = await UserModel.SignOutAsync();

                this.parameters?.ShowMessage?.Invoke(result.Message);
                this.parameters?.UpdateUserData?.Invoke(new UserDataModel());
            }
            catch (Exception ex)
            {
                this.parameters?.ShowMessage?.Invoke(ex.Message);
            }
            finally
            {
                viewModel.Loading = false;
            }
        }

        private void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }


        private async void MailConfig_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.Loading = true;
            await Dispatcher.Yield();
            this.parameters.ShowMessage(await UpdateProfile());
            this.viewModel.Loading = false;
        }

        private async ValueTask<string> UpdateProfile()
        {
            try
            {
                var result = await UserModel.UpdateProfileAsync(new ProfileUpdateModel
                {
                    Email = this.viewModel.UserData.Email,
                    HomePage = this.viewModel.UserData.HomePage,
                    NickName = this.viewModel.UserData.NickName,
                    Phone = this.viewModel.UserData.Phone,
                    MailConfig = this.viewModel.UserData.MailConfig
                });

                if (result.Status == StatusCode.OK)
                {
                    var model = UserModel.CurrentUser;
                    model.Phone = this.viewModel.UserData.Phone;
                    model.NickName = this.viewModel.UserData.NickName;
                    model.HomePage = this.viewModel.UserData.HomePage;
                    model.MailConfig = this.viewModel.UserData.MailConfig;
                    model.Email = this.viewModel.UserData.Email;
                    this.parameters.UpdateUserData(model);
                    UserModel.CurrentUser = model;
                }
                return result.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async void Profiles_Changed(object sender, RoutedEventArgs e)
        {
            this.viewModel.Loading = true;
            await Dispatcher.Yield();
            this.parameters.ShowMessage(await UpdateProfile());
            this.viewModel.Loading = false;
        }
    }
}
