#nullable enable
using Newtonsoft.Json;
using System.ComponentModel;

namespace MatrixUWP.Models.User
{
    public class UserDataModel : UserEssentialDataModel, INotifyPropertyChanged
    {
        private int userId;
        private bool captcha;
        private string nickName = "";
        private int isValid;
        private object? userAddition;
        private MailConfig? mailConfig;
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

        [JsonProperty("user_addition")]
        public object? UserAddition
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
        public MailConfig? MailConfig
        {
            get => mailConfig;
            set
            {
                mailConfig = value;
                OnPropertyChanged();
            }
        }

        public bool SignedIn => UserId != 0;
    }
}
