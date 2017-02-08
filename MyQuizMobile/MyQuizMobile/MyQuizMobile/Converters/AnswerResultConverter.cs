using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MyQuizMobile.Converters {
    class AnswerResultConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return (string)value == Constants.CorrectAnswerOptionText; }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return (bool)value ? Constants.CorrectAnswerOptionText : Constants.WrongAnswerOptionText; }
    }

    class AnswerResultTextConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return (string)value == Constants.CorrectAnswerOptionText ? "Richtig" : "Falsch"; }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }

    class QuestionTypeAnswerConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (string)value == Constants.QuestionTypeQuizText;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }
}
