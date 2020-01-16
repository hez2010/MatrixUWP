using MatrixUWP.Extensions;
using MatrixUWP.Models;
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
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ProfileParameters p)
            {
                parameters = p;
                viewModel.UserData = p.UserData;
            }
        }

        private async void SignOut_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Loading = true;
            await Dispatcher.YieldAsync();
            try
            {
                var result = await UserModel.SignOutAsync();

                parameters?.ShowMessage?.Invoke(result.Message);
                parameters?.UpdateUserData?.Invoke(new UserDataModel());
            }
            catch (Exception ex)
            {
                parameters?.ShowMessage?.Invoke(ex.Message);
                Debug.Fail(ex.Message, ex.StackTrace);
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
            parameters.ShowMessage(await UpdateProfile(stream));
            viewModel.Loading = false;
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
                        Email = viewModel.UserData.Email,
                        HomePage = viewModel.UserData.HomePage,
                        NickName = viewModel.UserData.NickName,
                        Phone = viewModel.UserData.Phone,
                        MailConfig = viewModel.UserData.MailConfig
                    })
                };

                if (result.Status == StatusCode.OK)
                {
                    var model = UserModel.CurrentUser;
                    model.Phone = viewModel.UserData.Phone;
                    model.NickName = viewModel.UserData.NickName;
                    model.HomePage = viewModel.UserData.HomePage;
                    model.MailConfig = viewModel.UserData.MailConfig;
                    model.Email = viewModel.UserData.Email;
                    parameters.UpdateUserData(model);
                    UserModel.CurrentUser = model;
                }
                return result.Message;
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message, ex.StackTrace);
                return ex.Message;
            }
        }

        private async void SaveProfiles_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Loading = true;
            await Dispatcher.YieldAsync();
            parameters.ShowMessage(await UpdateProfile());
            viewModel.Loading = false;
        }
    }
}
