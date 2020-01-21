#nullable enable
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI.Xaml;

namespace MatrixUWP.Models.Config
{
    class Configuration : INotifyPropertyChanged
    {
        private Language appLanguage = GetConfiguration(nameof(AppLanguage), Language.Default);
        private Theme appTheme = GetConfiguration(nameof(AppTheme), Theme.Default);
        private string savedUserName = GetConfiguration<string>(nameof(SavedUserName));
        private string savedPassword = GetConfiguration<string>(nameof(SavedPassword));

        public string SavedUserName
        {
            get => savedUserName;
            set
            {
                savedUserName = value;
                SaveConfiguration(value);
            }
        }
        public string SavedPassword
        {
            get => savedPassword;
            set
            {
                savedPassword = value;
                SaveConfiguration(value);
            }
        }

        public Language AppLanguage
        {
            get => appLanguage;
            set
            {
                if (!Enum.IsDefined(typeof(Language), value)) value = Language.Default;
                appLanguage = value;
                SaveConfiguration((int)value);
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
                SaveConfiguration((int)value);
                OnPropertyChanged(nameof(AppThemeValue));
            }
        }

        /// <summary>
        /// Binding value for RequestTheme
        /// </summary>
        public ElementTheme AppThemeValue => (ElementTheme)(int)AppTheme;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SaveConfiguration<T>(T value, [CallerMemberName] string? propertyName = null)
        {
            ApplicationData.Current.LocalSettings.Values[propertyName] = value;
        }

        protected static T GetConfiguration<T>([CallerMemberName] string? propertyName = null, T defaultValue = default!)
        {
            var value = ApplicationData.Current.LocalSettings.Values[propertyName];
            if (value is null) return defaultValue;
            return (T)value;
        }
    }
}
