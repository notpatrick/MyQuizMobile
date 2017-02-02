using System.Collections.Generic;

namespace MyQuizMobile.DataModel {
    public class QuestionBlock : MenuItem {
        public override int Id { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; set; }
        public override string DisplayText { get; set; }
        public override ItemType ItemType { get; set; }
    }
}