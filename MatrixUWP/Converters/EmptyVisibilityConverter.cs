#nullable enable
using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MatrixUWP.Converters
{
    internal class EmptyVisibilityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, string language)
        {
            if (value is string b) return string.IsNullOrEmpty(b) ? Visibility.Collapsed : Visibility.Visible;
            if (value is ICollection e) return e.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
            if (value is int i) return i == -1 ? Visibility.Collapsed : Visibility.Visible;
            if (value is long l) return l == -1 ? Visibility.Collapsed : Visibility.Visible;
            return value is null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotSupportedException();
    }
}
