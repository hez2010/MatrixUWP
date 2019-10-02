using MatrixUWP.Extensions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Threading.Tasks;
using MatrixUWP.Annotations;

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

    class MailConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool exam;
        private bool course;
        private bool library;
        private bool assignment;

        [JsonProperty("exam")]
        public bool Exam
        {
            get => exam;
            set
            {
                exam = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("course")]
        public bool Course
        {
            get => course;
            set
            {
                course = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("library")]
        public bool Library
        {
            get => library;
            set
            {
                library = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("course_assignment")]
        public bool Assignment
        {
            get => assignment;
            set
            {
                assignment = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
        private MailConfig mailConfig;
        private bool isInLib;
        private bool canCreateLib;

        [JsonProperty("captcha")]
        public bool Captcha
        {
            get => captcha;
            set
            {
                captcha = value;
                OnPropertyChanged();
            }
        }
        [JsonProperty("user_id")]
        public int UserId
        {
            get => userId;
            set
            {
                userId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SignedIn));
            }
        }
        [JsonProperty("nickname")]
        public string NickName
        {
            get => nickName;
            set
            {
                nickName = value;
                OnPropertyChanged();
            }
        }
        [JsonProperty("realname")]
        public string RealName
        {
            get => realName;
            set
            {
                realName = value;
                OnPropertyChanged();
            }
        }
        [JsonProperty("username")]
        public string UserName
        {
            get => userName;
            set
            {
                userName = value;
                OnPropertyChanged();
            }
        }
        [JsonProperty("is_valid")]
        public int IsValid
        {
            get => isValid;
            set
            {
                isValid = value;
                OnPropertyChanged();
            }
        }
        [JsonProperty("homepage")]
        public string HomePage
        {
            get => homePage;
            set
            {
                homePage = value;
                OnPropertyChanged();
            }
        }
        [JsonProperty("phone")]
        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged();
            }
        }
        [JsonProperty("email")]
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }
        [JsonProperty("user_addition")]
        public dynamic? UserAddition
        {
            get => userAddition;
            set
            {
                userAddition = value;
                OnPropertyChanged();
            }
        }
        [JsonProperty("isInLib")]
        public bool IsInLib
        {
            get => isInLib;
            set
            {
                isInLib = value;
                OnPropertyChanged();
            }
        }
        [JsonProperty("canCreateLib")]
        public bool CanCreateLib
        {
            get => canCreateLib;
            set
            {
                canCreateLib = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("mail_config")]
        public MailConfig MailConfig
        {
            get => mailConfig;
            set
            {
                mailConfig = value;
                OnPropertyChanged();
            }
        }

        public bool SignedIn => this.UserId != 0;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class UserModel
    {
        public static UserDataModel? CurrentUser { get; set; }
        public static async ValueTask<ResponseModel<UserDataModel?>> SignInAsync(string userName, string password, string captcha = "")
        {
            var result = await (string.IsNullOrEmpty(captcha) ?
                App.MatrixHttpClient.PostAsync("/api/users/login", new { username = userName, password })
                : App.MatrixHttpClient.PostAsync("/api/users/login", new { username = userName, password, captcha }))
            .JsonAsync<ResponseModel<UserDataModel?>>();
            if (result?.Data?.SignedIn ?? false)
            {
                App.AppConfiguration.SavedUserName = userName;
                App.AppConfiguration.SavedPassword = password;
                CurrentUser = result.Data;
            }
            else
            {
                App.AppConfiguration.SavedUserName = "";
                App.AppConfiguration.SavedPassword = "";
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
