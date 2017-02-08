using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public partial class SingleTopic : Item {
        [JsonIgnore]
        public bool IsVotingDone { get; set; } = false;
        public override string DisplayText { get { return Name; } set { Name = value; } }
        public override ItemType ItemType => ItemType.SingleTopic;
    }
}