namespace MyQuizMobile.DataModel {
    public partial class Question : Item {
        public override int Id { get; set; }
        public string Text { get; set; }
        public string Category { get; set; } //TODO: change to QuestionCategory enum once naming is clear
        //[JsonConverter(typeof(JsonMultipleChoiceConverter))]
        public string MultipleChoice { get; set; }
    }
}