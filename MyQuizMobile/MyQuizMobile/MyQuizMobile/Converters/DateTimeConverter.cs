using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class DateTimeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                return DateTime.Parse(value.ToString()).TimeOfDay;
            }
            return new TimeSpan(12, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return value?.ToString(); }
    }
}