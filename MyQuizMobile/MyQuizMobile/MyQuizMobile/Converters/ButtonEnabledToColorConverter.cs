using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class ButtonEnabledToColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool && (bool)value) {
                return Color.FromRgb(31, 174, 206);
            }
            return Color.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}