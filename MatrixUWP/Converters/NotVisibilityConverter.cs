#nullable enable
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MatrixUWP.Converters
{
    class NotVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b) return b ? Visibility.Collapsed : Visibility.Visible;
            throw new InvalidCastException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility v) return v != Visibility.Visible;
            throw new InvalidCastException();
        }
    }
}
