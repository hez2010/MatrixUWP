#nullable enable
using MatrixUWP.Models.Course.Assignment;
using MatrixUWP.Shared.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MatrixUWP.ViewModels
{
    class HomeViewModel : INotifyPropertyChanged, IDisposable
    {
        private SvgImageSource captchaData = new SvgImageSource();
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private bool captchaNeeded = false;
        private string userName = "";
        private string password = "";
        private string captcha = "";
        private bool signining;
        private List<ProgressingAssignmentModel> progressingAssignments = new List<ProgressingAssignmentModel>();
        private bool loading;

        public HomeViewModel()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, args) => OnPropertyChanged(nameof(CurrentDateTime));
            timer.Start();
        }

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
                if (string.IsNullOrEmpty(userName))
                {
                    bitmap.UriSource = new Uri("ms-appx:///Assets/Home/user.png");
                }
                else
                {
                    try
                    {
                        bitmap.UriSource = new Uri(HttpUtils.MatrixHttpClient.BaseUri, $"/api/users/profile/avatar?username={userName}&t={DateTime.Now.Ticks}");
                    }
                    catch
                    {
                        bitmap.UriSource = new Uri("ms-appx:///Assets/Home/user.png");
                    }
                }

                return bitmap;
            }
        }

        public bool Signining
        {
            get => signining;
            set
            {
                signining = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SignInButtonEnabled));
            }
        }

        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }

        public bool SignInButtonEnabled => !signining && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password);

        public bool CaptchaNeeded
        {
            get => captchaNeeded;
            set
            {
                captchaNeeded = value;
                OnPropertyChanged();
            }
        }

        public List<ProgressingAssignmentModel> ProgressingAssignments
        {
            get => progressingAssignments;
            set
            {
                progressingAssignments = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasProgressingAssignments));
            }
        }

        public bool HasProgressingAssignments => ProgressingAssignments.Count != 0;

        public string CurrentDateTime => DateTime.Now.ToString("yyyy ”N MM ŒŽ dd “ú");

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Dispose()
        {
            timer.Stop();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
