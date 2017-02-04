using System;
using System.Globalization;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class ItemTypeToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var input = (ItemType)value;
            var result = string.Empty;
            switch (input) {
            case ItemType.None:
                result = "None";
                break;
            case ItemType.Group:
                result = "Veranstaltung";
                break;
            case ItemType.QuestionBlock:
                result = "Frageliste";
                break;
            case ItemType.Question:
                result = "Frage";
                break;
            case ItemType.AnswerOption:
                result = "Antwortmöglichkeit";
                break;
            case ItemType.SingleTopic:
                result = "Person";
                break;
            case ItemType.Device:
                result = "Gerät";
                break;
            case ItemType.GivenAnswer:
                result = "Ergebnis";
                break;
            default:
                break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return null; }
    }
}