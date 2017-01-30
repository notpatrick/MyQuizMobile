using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyQuizMobile
{
    public class PersonenbezogenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool) value)
            {
                return "Fortfahren";
            }
            return "Senden";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
