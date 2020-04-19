#nullable enable
using System;
using Windows.UI.Xaml.Data;

namespace MatrixUWP.Converters
{
    public class LanguageConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is null) return value;
            var str = value.ToString().ToLower();
            return str switch
            {
                "c" => "cpp",
                "c++" => "cpp",
                "g++" => "cpp",
                "clang++" => "cpp",
                "gcc" => "cpp",
                "clang" => "cpp",
                "c#" => "csharp",
                "f#" => "fsharp",
                "python2" => "python",
                "python3" => "python",
                _ => str
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
