using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class InvertBoolConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var booleanValue = (bool)value;
            return !booleanValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            var booleanValue = (bool)value;
            return !booleanValue;
        }
    }

    public class IntToBoolConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return (long)value != 0; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }

    public class ValueConverterGroup : List<IValueConverter>, IValueConverter {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture)); }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
        #endregion
    }
}