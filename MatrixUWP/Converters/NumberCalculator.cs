#nullable enable
using System;
using Windows.UI.Xaml.Data;

namespace MatrixUWP.Converters
{
    internal class NumberCalculator : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, string language) => value == null || parameter == null
                ? value
                : System.Convert.ChangeType(double.Parse(value.ToString()) + double.Parse(parameter.ToString()), targetType);

        public object? ConvertBack(object value, Type targetType, object parameter, string language) => value == null || parameter == null
                ? value
                : System.Convert.ChangeType(double.Parse(value.ToString()) - double.Parse(parameter.ToString()), targetType);
    }
}
