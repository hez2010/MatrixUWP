#nullable enable
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatrixUWP.Models.User
{
    public class CaptchaDataModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string captcha = "";

        [JsonProperty("captcha")]
        public string Captcha
        {
            get => captcha;
            set
            {
                captcha = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
