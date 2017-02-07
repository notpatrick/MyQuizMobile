using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class IntStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return $"{value}"; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            var input = value.ToString();
            int result;
            int.TryParse(input, out result);
            return result;
        }
    }
}