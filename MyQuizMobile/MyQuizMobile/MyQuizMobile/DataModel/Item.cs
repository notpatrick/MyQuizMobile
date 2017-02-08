using System.ComponentModel;
using Newtonsoft.Json;
using PostSharp.Patterns.Model;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public abstract class Item {
        [DefaultValue(0)]
        public abstract long Id { get; set; }
        [JsonIgnore]
        public virtual string DisplayText { get; set; }
        [JsonIgnore]
        public virtual ItemType ItemType { get; set; }
        [JsonIgnore]
        public virtual string DetailText { get; set; }
    }

    public class MenuItem : Item{
        public override long Id { get; set; }
        public override string DisplayText { get; set; }
        public override ItemType ItemType { get; set; }
        public override string DetailText { get; set; }
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