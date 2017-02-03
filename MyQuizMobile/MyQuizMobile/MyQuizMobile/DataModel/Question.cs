using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public class Question : MenuItem {
        public override int Id { get; set; }
        public string Text { get; set; }
        public QuestionCategory Category { get; set; }
        public bool MultipleChoice { get; set; }
        [JsonIgnore]
        public virtual List<AnswerOption> AnswerOptions { get; set; }
        public override string DisplayText { get { return Text; } set { Text = value; } }
        public override ItemType ItemType => ItemType.Question;
    }

    public enum QuestionCategory {
        Umfrage,
        Quiz
    }
}