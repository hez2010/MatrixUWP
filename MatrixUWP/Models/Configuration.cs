using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI.Xaml;

namespace MatrixUWP.Models
{
    public class Configuration : INotifyPropertyChanged
    {
        private int appLanguage = (int)(ApplicationData.Current.LocalSettings.Values["AppLanguage"] ?? 0);
        private int appTheme = (int)(ApplicationData.Current.LocalSettings.Values["AppTheme"] ?? 0);

        /// <summary>
        /// Language: 0 - default, 1 - en-us, 2 - zh-cn
        /// </summary>
        public int AppLanguage
        {
            get => appLanguage;
            set
            {
                if (value < 0 || value > 2) value = 0;
                ApplicationData.Current.LocalSettings.Values["AppLanguage"] = appLanguage = value;
                ApplicationLanguages.PrimaryLanguageOverride = value switch
                {
                    1 => "en-us",
                    2 => "zh-cn",
                    _ => ""
                };
            }
        }

        /// <summary>
        /// Theme: 0 - default, 1 - light, 2 - dark
        /// </summary>
        public int AppTheme
        {
            get => appTheme;
            set
            {
                if (value < 0 || value > 2) value = 0;
                ApplicationData.Current.LocalSettings.Values["AppTheme"] = appTheme = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AppThemeValue)));


            }
        }

        /// <summary>
        /// Binding value for RequestTheme
        /// </summary>
        public ElementTheme AppThemeValue => (ElementTheme)AppTheme;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
