#nullable enable
﻿using System;
using Windows.UI.Xaml.Data;

namespace MatrixUWP.Converters
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Enum)
            {
                return System.Convert.ChangeType(value, targetType);
            }
            throw new InvalidCastException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (Enum.IsDefined(targetType, value))
            {
                return Enum.ToObject(targetType, value);
            }
            throw new InvalidCastException();
        }
    }
}
