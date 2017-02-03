using System;

namespace MyQuizMobile.DataModel {
    public class GivenAnswer : MenuItem {
        public DateTime TimeStamp { get; set; }
        public QuestionBlock QuestionBlock { get; set; }
        public Question Question { get; set; }
        public AnswerOption AnswerOption { get; set; }
        public SingleTopic SingleTopic { get; set; }
        public Group Group { get; set; }
        public Device Device { get; set; }
        public override int Id { get; set; }
        public override string DisplayText => $"Given Answer {Id}";
        public override ItemType ItemType => ItemType.GivenAnswer;
    }

    public class IdGivenAnswer {
        public DateTime TimeStamp { get; set; }
        public int? QuestionBlockId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerOptionId { get; set; }
        public int? SingleTopicId { get; set; }
        public int GroupId { get; set; }
        public int? DeviceId { get; set; }
    }
}