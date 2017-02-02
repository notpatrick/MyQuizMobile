namespace MyQuizMobile.DataModel {
    public class Device : MenuItem {
        public override int Id { get; set; }
        public string PushUpToken { get; set; }
        public bool IsAdmin { get; set; }
        public override string DisplayText { get; set; }
        public override ItemType ItemType { get; set; }
    }
}