using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.UI.Xaml;

namespace MatrixUWP.Models
{
    public class Configuration
    {
        private int appLanguage;
        private int appTheme;

        public int AppLanguage
        {
            get => appLanguage;
            set
            {
                appLanguage = value;
                ApplicationLanguages.PrimaryLanguageOverride = value switch
                {
                    0 => "en-US",
                    1 => "zh-CN",
                    _ => "en-US"
                };
            }
        }
        public int AppTheme
        {
            get => appTheme;
            set
            {
                appTheme = value;
            }
        }
    }
}
