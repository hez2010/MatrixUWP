#nullable enable
using MatrixUWP.Models;
using MatrixUWP.Models.User;
using MatrixUWP.Shared.Extensions;
using MatrixUWP.Shared.Models;
using MatrixUWP.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MatrixUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Profile : Page
    {
        private readonly ProfileViewModel viewModel = new ProfileViewModel();
        public Profile()
        {
            InitializeComponent();
            viewModel.UserData = new UserDataModel();
            UserModel.CurrentUser?.CopyTo(viewModel.UserData);
        }

        private async void SignOut_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Loading = true;

            try
            {
                var result = await UserModel.SignOutAsync();

                AppModel.ShowMessage?.Invoke(result?.Message ?? "发生错误");
                UserModel.UpdateUserData(new UserDataModel());
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

        private async void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            var file = await picker.PickSingleFileAsync();
            if (file is null) return;
            viewModel.Loading = true;

            AppModel.ShowMessage?.Invoke(await UpdateProfile(file));
            viewModel.Loading = false;
        }

        private async ValueTask<string> UpdateProfile(StorageFile? file = null)
        {
            if (viewModel.UserData is null) throw new NullReferenceException("UserData cannot be null.");
            try
            {
                var result = file switch
                {
                    StorageFile _ => await UserModel.UpdateAvatarAsync(file),
                    _ => await UserModel.UpdateProfileAsync(new ProfileUpdateModel
                    {
                        Email = viewModel.UserData.Email,
                        HomePage = viewModel.UserData.HomePage,
                        NickName = viewModel.UserData.NickName,
                        Phone = viewModel.UserData.Phone,
                        MailConfig = viewModel.UserData.MailConfig
                    })
                };
                if (file != null) viewModel.UserData.OnPropertyChanged(nameof(viewModel.UserData.Avatar));
                if (result?.Status == StatusCode.OK)
                {
                    UserModel.UpdateUserData(viewModel.UserData);
                }
                return result?.Message ?? "发生错误";
            }
            catch (Exception ex)
            {
#if FAIL_ON_DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
                return ex.Message;
            }
        }

        private async void SaveProfiles_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Loading = true;

            AppModel.ShowMessage?.Invoke(await UpdateProfile());
            viewModel.Loading = false;
        }
    }
}
