using MatrixUWP.Extensions;
using MatrixUWP.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MatrixUWP.Models
{
    class CaptchaDataModel : INotifyPropertyChanged
    {
        private string captcha = "";

        [JsonPropertyName("captcha")]
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
        private dynamic userAddition;
        private string studentId = "";
        private string academy = "";
        private string specialty = "";
        private bool isInLib;
        private bool canCreateLib;

        [JsonPropertyName("captcha")]
        public bool Captcha
        {
            get => captcha;
            set
            {
                captcha = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Captcha)));
            }
        }
        [JsonPropertyName("user_id")]
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
        [JsonPropertyName("nickname")]
        public string NickName
        {
            get => nickName;
            set
            {
                nickName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NickName)));
            }
        }
        [JsonPropertyName("realname")]
        public string RealName
        {
            get => realName;
            set
            {
                realName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RealName)));
            }
        }
        [JsonPropertyName("username")]
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
            }
        }
        [JsonPropertyName("is_valid")]
        public int IsValid
        {
            get => isValid; set
            {
                isValid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
            }
        }
        [JsonPropertyName("homepage")]
        public string HomePage
        {
            get => homePage; set
            {
                homePage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HomePage)));
            }
        }
        [JsonPropertyName("phone")]
        public string Phone
        {
            get => phone; set
            {
                phone = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Phone)));
            }
        }
        [JsonPropertyName("email")]
        public string Email
        {
            get => email;
            set
            {
                email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
            }
        }
        [JsonPropertyName("user_addition")]
        public dynamic? UserAddition
        {
            get => userAddition;
            set
            {
                userAddition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserAddition)));
            }
        }
        [JsonPropertyName("student_id")]
        public string StudentId
        {
            get => studentId;
            set
            {
                studentId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StudentId)));
            }
        }
        [JsonPropertyName("academy")]
        public string Academy
        {
            get => academy;
            set
            {
                academy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Academy)));
            }
        }
        [JsonPropertyName("specialty")]
        public string Specialty
        {
            get => specialty;
            set
            {
                specialty = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Specialty)));
            }
        }
        [JsonPropertyName("isInLib")]
        public bool IsInLib
        {
            get => isInLib;
            set
            {
                isInLib = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsInLib)));
            }
        }
        [JsonPropertyName("canCreateLib")]
        public bool CanCreateLib
        {
            get => canCreateLib;
            set
            {
                canCreateLib = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanCreateLib)));
            }
        }

        public bool SignedIn => this.UserId != 0;

        public event PropertyChangedEventHandler PropertyChanged;
    }

    class UserModel
    {
        public static Task<ResponseModel<UserDataModel>> SignInAsync(string userName, string password, string captcha = "")
            => (string.IsNullOrEmpty(captcha) ?
                App.MatrixHttpClient.PostAsync("/api/users/login", new { username = userName, password = password })
                : App.MatrixHttpClient.PostAsync("/api/users/login", new { username = userName, password = password, captcha = captcha }))
            .JsonAsync<ResponseModel<UserDataModel>>();

        public static Task<ResponseModel<CaptchaDataModel>> FetchCaptchaAsync() => App.MatrixHttpClient.GetAsync(
                "/api/captcha"
            ).JsonAsync<ResponseModel<CaptchaDataModel>>();
    }
}
