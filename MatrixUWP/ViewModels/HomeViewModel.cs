#nullable enable
using MatrixUWP.Utils;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MatrixUWP.ViewModels
{
    class HomeViewModel : INotifyPropertyChanged
    {
        private SvgImageSource captchaData = new SvgImageSource();
        private bool captchaNeeded = false;
        private string userName = "";
        private string password = "";
        private string captcha = "";
        private bool loading;

        public SvgImageSource CaptchaData
        {
            get => captchaData;
            set
            {
                captchaData = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Avatar));
                OnPropertyChanged(nameof(SignInButtonEnabled));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SignInButtonEnabled));
            }
        }

        public string Captcha
        {
            get => captcha;
            set
            {
                captcha = value;
                OnPropertyChanged();
            }
        }

        public ImageSource Avatar
        {
            get
            {
                var bitmap = new BitmapImage();
                if (string.IsNullOrEmpty(userName)) bitmap.UriSource = new Uri("ms-appx:///Assets/Home/user.png");
                else
                {
                    try
                    {
                        bitmap.UriSource = new Uri(MatrixJsonHttpRequestBuilder.BaseUri, $"/api/users/profile/avatar?username={userName}");
                    }
                    catch
                    {
                        bitmap.UriSource = new Uri("ms-appx:///Assets/Home/user.png");
                    }
                }

                return bitmap;
            }
        }

        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SignInButtonEnabled));
            }
        }

        public bool SignInButtonEnabled => !loading && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password);

        public bool CaptchaNeeded
        {
            get => captchaNeeded;
            set
            {
                captchaNeeded = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
