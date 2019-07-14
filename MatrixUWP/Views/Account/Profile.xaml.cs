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
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MatrixUWP.Views.Account
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Profile : Page
    {
        private readonly ProfileViewModel viewModel = new ProfileViewModel();
        private ProfileParameters parameters = new ProfileParameters();
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
                await TryHelper.Try(async () =>
                {
                    var result = await UserModel.SignOutAsync();

                    Debug.WriteLine(result.SerializeJson());
                    this.parameters.ShowMessage?.Invoke(result.Message);

                    this.parameters.UpdateUserData?.Invoke(new UserDataModel());
                });
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

        private void ChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            // TODO
            Debugger.Break();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
