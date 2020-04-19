#nullable enable
using System;
using Windows.UI.Xaml.Data;

namespace MatrixUWP.Converters
{
    internal class NumberLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => value is int val ? System.Convert.ChangeType((char)('A' + val), targetType) : value;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => value is int val
                ? System.Convert.ChangeType(val - 'A', targetType)
                : value is string strVal ? System.Convert.ChangeType(char.Parse(strVal) - 'A', targetType) : value;
    }
}
