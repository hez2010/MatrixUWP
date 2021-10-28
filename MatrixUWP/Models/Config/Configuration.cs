#nullable enable
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.UI.Xaml;

namespace MatrixUWP.Models.Config
{
    internal class Configuration : INotifyPropertyChanged
    {
        private ElementTheme appTheme = GetConfiguration(nameof(AppTheme), ElementTheme.Default);
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

        public ElementTheme AppTheme
        {
            get => appTheme;
            set
            {
                if (!Enum.IsDefined(typeof(ElementTheme), value)) value = ElementTheme.Default;
                appTheme = value;
                SaveConfiguration((int)value);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void SaveConfiguration<T>(T value, [CallerMemberName] string? propertyName = null) => ApplicationData.Current.LocalSettings.Values[propertyName] = JsonConvert.SerializeObject(value);

        protected static T GetConfiguration<T>([CallerMemberName] string? propertyName = null, T defaultValue = default!)
        {
            var value = ApplicationData.Current.LocalSettings.Values[propertyName];
            if (value is null) return defaultValue;
            try
            {
                return JsonConvert.DeserializeObject<T>(value.ToString()) ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
