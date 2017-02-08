namespace MyQuizMobile.DataModel {
    public partial class Question : Item {
        public override long Id { get; set; }
        public string Text { get; set; }
        public string Category { get; set; } = Constants.QuestionCategoryVoteText;
        public string Type { get; set; } = Constants.QuestionTypeSingleText;
    }
}