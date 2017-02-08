namespace MyQuizMobile.DataModel {
    public partial class AnswerOption {
        public override long Id { get; set; }
        public string Text { get; set; }
        public string Result { get; set; } = Constants.WrongAnswerOptionText;
    }
}