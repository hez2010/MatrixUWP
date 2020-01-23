#nullable enable
using MatrixUWP.Extensions;
using MatrixUWP.Models;
using MatrixUWP.Models.User;
using MatrixUWP.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MatrixUWP.Views.Account
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
            await Dispatcher.YieldAsync();
            try
            {
                var result = await UserModel.SignOutAsync();

                AppModel.ShowMessage?.Invoke(result.Message);
                UserModel.UpdateUserData(new UserDataModel());
            }
            catch (Exception ex)
            {
                AppModel.ShowMessage?.Invoke(ex.Message);
#if DEBUG
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
            picker.FileTypeFilter.Add(".gif");
            var file = await picker.PickSingleFileAsync();
            if (file == null) return;
            using var stream = await file.OpenSequentialReadAsync();
            viewModel.Loading = true;
            await Dispatcher.YieldAsync();
            AppModel.ShowMessage?.Invoke(await UpdateProfile(stream));
            viewModel.Loading = false;
        }

        private async ValueTask<string> UpdateProfile(IInputStream? stream = null)
        {
            if (viewModel.UserData is null) throw new NullReferenceException("UserData cannot be null.");
            try
            {
                var result = stream switch
                {
                    IInputStream _ => await UserModel.UpdateAvatarAsync(stream),
                    _ => await UserModel.UpdateProfileAsync(new ProfileUpdateModel
                    {
                        Email = viewModel.UserData.Email,
                        HomePage = viewModel.UserData.HomePage,
                        NickName = viewModel.UserData.NickName,
                        Phone = viewModel.UserData.Phone,
                        MailConfig = viewModel.UserData.MailConfig
                    })
                };

                if (result.Status == StatusCode.OK)
                {
                    UserModel.UpdateUserData(viewModel.UserData);
                }
                return result.Message;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.Fail(ex.Message, ex.StackTrace);
#endif
                return ex.Message;
            }
        }

        private async void SaveProfiles_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Loading = true;
            await Dispatcher.YieldAsync();
            AppModel.ShowMessage?.Invoke(await UpdateProfile());
            viewModel.Loading = false;
        }
    }
}
