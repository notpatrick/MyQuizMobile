namespace MyQuizMobile.DataModel {
    public class AnswerOption : MenuItem {
        public AnswerOption() { DisplayText = $"{Text}"; }

        public override int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public override string DisplayText { get; set; }
        public override ItemType ItemType { get; set; }
    }
}