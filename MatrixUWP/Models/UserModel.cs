using MatrixUWP.Extensions;
using MatrixUWP.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MatrixUWP.Models
{
    class CaptchaDataModel : INotifyPropertyChanged
    {
        private string captcha = "";

        [JsonProperty("captcha")]
        public string Captcha
        {
            get => captcha;
            set
            {
                captcha = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Captcha)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    class UserDataModel : INotifyPropertyChanged
    {
        private bool captcha;
        private int userId;
        private string nickName = "";
        private string realName = "";
        private string userName = "";
        private int isValid;
        private string homePage = "";
        private string phone = "";
        private string email = "";
        private dynamic? userAddition;
        private string studentId = "";
        private string academy = "";
        private string specialty = "";
        private bool isInLib;
        private bool canCreateLib;
        private dynamic mailConfig;

        [JsonProperty("captcha")]
        public bool Captcha
        {
            get => captcha;
            set
            {
                captcha = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Captcha)));
            }
        }
        [JsonProperty("user_id")]
        public int UserId
        {
            get => userId;
            set
            {
                userId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserId)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SignedIn)));
            }
        }
        [JsonProperty("nickname")]
        public string NickName
        {
            get => nickName;
            set
            {
                nickName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NickName)));
            }
        }
        [JsonProperty("realname")]
        public string RealName
        {
            get => realName;
            set
            {
                realName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RealName)));
            }
        }
        [JsonProperty("username")]
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
            }
        }
        [JsonProperty("is_valid")]
        public int IsValid
        {
            get => isValid; set
            {
                isValid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
            }
        }
        [JsonProperty("homepage")]
        public string HomePage
        {
            get => homePage; set
            {
                homePage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HomePage)));
            }
        }
        [JsonProperty("phone")]
        public string Phone
        {
            get => phone; set
            {
                phone = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Phone)));
            }
        }
        [JsonProperty("email")]
        public string Email
        {
            get => email;
            set
            {
                email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
            }
        }
        [JsonProperty("user_addition")]
        public dynamic? UserAddition
        {
            get => userAddition;
            set
            {
                userAddition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserAddition)));
            }
        }
        [JsonProperty("student_id")]
        public string StudentId
        {
            get => studentId;
            set
            {
                studentId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StudentId)));
            }
        }
        [JsonProperty("academy")]
        public string Academy
        {
            get => academy;
            set
            {
                academy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Academy)));
            }
        }
        [JsonProperty("specialty")]
        public string Specialty
        {
            get => specialty;
            set
            {
                specialty = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Specialty)));
            }
        }
        [JsonProperty("isInLib")]
        public bool IsInLib
        {
            get => isInLib;
            set
            {
                isInLib = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsInLib)));
            }
        }
        [JsonProperty("canCreateLib")]
        public bool CanCreateLib
        {
            get => canCreateLib;
            set
            {
                canCreateLib = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanCreateLib)));
            }
        }

        [JsonProperty("mail_config")]
        public dynamic MailConfig
        {
            get => mailConfig;
            set
            {
                mailConfig = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MailConfig)));
            }
        }

        public bool SignedIn => this.UserId != 0;

        public event PropertyChangedEventHandler PropertyChanged;
    }

    class UserModel
    {
        public static UserDataModel? CurrentUser { get; set; }
        public static async ValueTask<ResponseModel<UserDataModel>> SignInAsync(string userName, string password, string captcha = "")
        {
            var result = await (string.IsNullOrEmpty(captcha) ?
                App.MatrixHttpClient.PostAsync("/api/users/login", new { username = userName, password })
                : App.MatrixHttpClient.PostAsync("/api/users/login", new { username = userName, password, captcha }))
            .JsonAsync<ResponseModel<UserDataModel>>();
            if (result?.Data?.SignedIn ?? false)
            {
                App.AppConfiguration.SavedUserName = userName;
                App.AppConfiguration.SavedPassword = password;
                CurrentUser = result.Data;
            }
            return result;
        }

        public static ValueTask<ResponseModel<CaptchaDataModel>> FetchCaptchaAsync() => App.MatrixHttpClient.GetAsync(
                "/api/captcha"
            ).JsonAsync<ResponseModel<CaptchaDataModel>>();

        public static ValueTask<ResponseModel> SignOutAsync()
        {
            App.AppConfiguration.SavedUserName = "";
            App.AppConfiguration.SavedPassword = "";
            CurrentUser = null;
            return App.MatrixHttpClient.PostAsync("/api/users/logout", new { }).JsonAsync<ResponseModel>();
        }
    }
}
