using MatrixUWP.Models;
using MatrixUWP.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MatrixUWP.ViewModels
{
    class ProfileViewModel : INotifyPropertyChanged
    {
        private bool loading;
        private UserDataModel userData = new UserDataModel();

        public UserDataModel UserData
        {
            get => userData;
            set
            {
                this.userData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserData)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Avatar)));
            }
        }

        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Loading)));
            }
        }

        public ImageSource Avatar
        {
            get
            {
                var bitmap = new BitmapImage();
                if (string.IsNullOrEmpty(userData?.UserName)) bitmap.UriSource = new Uri("ms-appx:///Assets/Home/user.png");
                else
                {
                    try
                    {
                        bitmap.UriSource = new Uri(MatrixJsonHttpRequestBuilder.baseUri, $"/api/users/profile/avatar?username={userData.UserName}");
                    }
                    catch
                    {
                        bitmap.UriSource = new Uri("ms-appx:///Assets/Home/user.png");
                    }
                }

                return bitmap;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
