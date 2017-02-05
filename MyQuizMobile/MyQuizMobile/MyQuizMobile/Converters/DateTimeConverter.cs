using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MyQuizMobile
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var input = (DateTime?)value;
            return input?.TimeOfDay ?? TimeSpan.FromHours(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            var result = DateTime.ParseExact(value.ToString(), "HH:mm:ss", null);
            return result;
        }
    }
}
