namespace MyQuizMobile.DataModel {
    public class AnswerOption : MenuItem {
        public override int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public override string DisplayText { get { return Text; } set { Text = value; } }
        public override ItemType ItemType => ItemType.AnswerOption;
    }
}