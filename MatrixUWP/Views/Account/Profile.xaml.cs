using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.Utils;
using MatrixUWP.ViewModels;
using MatrixUWP.Views.Parameters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
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

        private async void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");
            var file = await picker.PickSingleFileAsync();
            if (file == null) return;
            using var stream = await file.OpenSequentialReadAsync();
            this.viewModel.Loading = true;
            await Dispatcher.Yield();
            this.parameters.ShowMessage(await UpdateProfile(stream));
            this.viewModel.Loading = false;
        }

        private async ValueTask<string> UpdateProfile(IInputStream? stream = null)
        {
            try
            {
                var result = stream switch
                {
                    IInputStream _ => await UserModel.UpdateAvatarAsync(stream),
                    _ => await UserModel.UpdateProfileAsync(new ProfileUpdateModel
                    {
                        Email = this.viewModel.UserData.Email,
                        HomePage = this.viewModel.UserData.HomePage,
                        NickName = this.viewModel.UserData.NickName,
                        Phone = this.viewModel.UserData.Phone,
                        MailConfig = this.viewModel.UserData.MailConfig
                    })
                };

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

        private async void SaveProfiles_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.Loading = true;
            await Dispatcher.Yield();
            this.parameters.ShowMessage(await UpdateProfile());
            this.viewModel.Loading = false;
        }
    }
}
