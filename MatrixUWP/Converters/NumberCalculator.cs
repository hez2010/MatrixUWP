using System;
using Windows.UI.Xaml.Data;

namespace MatrixUWP.Converters
{
    class NumberCalculator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null) return value;
            return System.Convert.ChangeType(double.Parse(value.ToString()) + double.Parse(parameter.ToString()), targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null) return value;
            return System.Convert.ChangeType(double.Parse(value.ToString()) - double.Parse(parameter.ToString()), targetType);
        }
    }
}
