using System.Collections.ObjectModel;

namespace MyQuizMobile.DataModel {
    public class Group : MenuItem {
        public override int Id { get; set; }
        public string Title { get; set; }
        public string EnterGroupPin { get; set; }
        public ObservableCollection<SingleTopic> SingleTopics { get; set; }
        public override string DisplayText { get; set; }
        public override ItemType ItemType { get; set; }

        public void SetDisplayText() { DisplayText = $"{Title}"; }
    }
}