using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyQuizMobile.Converters {
    internal class AnswerResultConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return (string)value == Constants.CorrectAnswerOptionText; }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return (bool)value ? Constants.CorrectAnswerOptionText : Constants.WrongAnswerOptionText; }
    }

    internal class AnswerResultTextConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return (string)value == Constants.CorrectAnswerOptionText ? "Richtig" : "Falsch"; }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }

    internal class QuestionTypeAnswerConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return (string)value == Constants.QuestionCategoryQuizText; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }
}