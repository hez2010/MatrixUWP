using MatrixUWP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MatrixUWP.ViewModels
{
    class HomeViewModel : INotifyPropertyChanged
    {
        private SvgImageSource captchaData = new SvgImageSource();
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CaptchaData)));
            }
        }

        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SignInButtonEnabled)));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SignInButtonEnabled)));
            }
        }

        public string Captcha
        {
            get => captcha;
            set
            {
                captcha = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Captcha)));
            }
        }

        public bool Loading
        {
            get => loading;
            set
            {
                loading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Loading)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SignInButtonEnabled)));
            }
        }

        public bool SignInButtonEnabled => !this.loading && !string.IsNullOrEmpty(this.userName) && !string.IsNullOrEmpty(this.password);

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
