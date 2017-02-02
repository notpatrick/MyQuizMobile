using System.Collections.Generic;

namespace MyQuizMobile.DataModel
{
    public class Question : MenuItem
    {
        public override int Id { get; set; }
        public string Text { get; set; }
        public QuestionCategory Category { get; set; }
        public bool MultipleChoice { get; set; }

        public virtual List<AnswerOption> AnswerOptions { get; set; }
        public override string DisplayText { get; set; }
        public override ItemType ItemType { get; set; }
    }

    public enum QuestionCategory
    {
        Umfrage,
        Quiz
    }
}
