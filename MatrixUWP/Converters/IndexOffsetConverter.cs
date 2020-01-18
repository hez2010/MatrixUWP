#nullable enable
using System;
using Windows.UI.Xaml.Data;

namespace MatrixUWP.Converters
{
    class IndexOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int val) return System.Convert.ChangeType(val + 1, targetType);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is int val) return System.Convert.ChangeType(val - 1, targetType);
            if (value is string strVal) return System.Convert.ChangeType(int.Parse(strVal) - 1, targetType);
            return value;
        }
    }
}
