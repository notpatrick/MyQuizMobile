using Newtonsoft.Json;
using PostSharp.Patterns.Model;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public abstract class MenuItem {
        public abstract int Id { get; set; }
        [JsonIgnore]
        public virtual string DisplayText { get; set; }
        [JsonIgnore]
        public virtual ItemType ItemType { get; set; }
        [JsonIgnore]
        public virtual string DetailText { get; set; }
    }

    public enum ItemType {
        None,
        Group,
        QuestionBlock,
        Question,
        AnswerOption,
        SingleTopic,
        Device,
        GivenAnswer
    }
}