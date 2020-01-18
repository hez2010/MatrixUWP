#nullable enable
﻿using System;
using Windows.UI.Xaml.Data;

namespace MatrixUWP.Converters
{
    class NotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b) return !b;
            throw new InvalidCastException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b) return !b;
            throw new InvalidCastException();
        }
    }
}
