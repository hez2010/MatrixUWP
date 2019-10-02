using System;
using System.ComponentModel;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI.Xaml;

namespace MatrixUWP.Models
{
    enum Language : int
    {
        Default, English, Chinese
    }
    enum Theme : int
    {
        Default, Light, Dark
    }
    class Configuration : INotifyPropertyChanged
    {
        private Language appLanguage = (Language)(ApplicationData.Current.LocalSettings.Values["AppLanguage"] ?? 0);
        private Theme appTheme = (Theme)(ApplicationData.Current.LocalSettings.Values["AppTheme"] ?? 0);
        private string savedUserName = (string)ApplicationData.Current.LocalSettings.Values["SavedUserName"];
        private string savedPassword = (string)ApplicationData.Current.LocalSettings.Values["SavedPassword"];

        public string SavedUserName
        {
            get => savedUserName;
            set
            {
                savedUserName = value;
                ApplicationData.Current.LocalSettings.Values["SavedUserName"] = value;
            }
        }
        public string SavedPassword
        {
            get => savedPassword;
            set
            {
                savedPassword = value;
                ApplicationData.Current.LocalSettings.Values["SavedPassword"] = value;
            }
        }

        public Language AppLanguage
        {
            get => appLanguage;
            set
            {
                if (!Enum.IsDefined(typeof(Language), value)) value = Language.Default;
                appLanguage = value;
                ApplicationData.Current.LocalSettings.Values["AppLanguage"] = (int)value;
                ApplicationLanguages.PrimaryLanguageOverride = value switch
                {
                    Language.English => "en-us",
                    Language.Chinese => "zh-cn",
                    _ => ""
                };
            }
        }

        public Theme AppTheme
        {
            get => appTheme;
            set
            {
                if (!Enum.IsDefined(typeof(Theme), value)) value = Theme.Default;
                appTheme = value;
                ApplicationData.Current.LocalSettings.Values["AppTheme"] = (int)value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AppThemeValue)));
            }
        }

        /// <summary>
        /// Binding value for RequestTheme
        /// </summary>
        public ElementTheme AppThemeValue => (ElementTheme)(int)AppTheme;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
