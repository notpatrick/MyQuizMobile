﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyQuizMobile
{
    public class SendButtonColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool) value)
            {
                return Xamarin.Forms.Color.FromRgb(31, 174, 206);
            }
            return Xamarin.Forms.Color.Default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
