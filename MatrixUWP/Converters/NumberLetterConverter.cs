#nullable enable
using System;
using Windows.UI.Xaml.Data;

namespace MatrixUWP.Converters
{
    class NumberLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int val) return System.Convert.ChangeType((char)('A' + val), targetType);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is int val) return System.Convert.ChangeType(val - 'A', targetType);
            if (value is string strVal) return System.Convert.ChangeType(char.Parse(strVal) - 'A', targetType);
            return value;
        }
    }
}
