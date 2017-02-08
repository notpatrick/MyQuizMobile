using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public partial class AnswerOption : Item {
        public override string DisplayText { get { return Text; } set { Text = value; } }
        public override ItemType ItemType => ItemType.AnswerOption;
        [JsonIgnore]
        public bool IsCorrect { get { return Result == Constants.CorrectAnswerOptionText; } set { Result = value ? Constants.CorrectAnswerOptionText : Constants.WrongAnswerOptionText; } }

        [JsonIgnore]
        public int Count { get; set; } = 0;
    }
}