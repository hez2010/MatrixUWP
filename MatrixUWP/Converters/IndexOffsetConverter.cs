#nullable enable
using System;
using Windows.UI.Xaml.Data;

namespace MatrixUWP.Converters
{
    internal class IndexOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => value is int val ? System.Convert.ChangeType(val + 1, targetType) : value;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => value is int val
                ? System.Convert.ChangeType(val - 1, targetType)
                : value is string strVal ? System.Convert.ChangeType(int.Parse(strVal) - 1, targetType) : value;
    }
}
