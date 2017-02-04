namespace MyQuizMobile.DataModel {
    public partial class Device {
        public override int Id { get; set; }
        public string PushUpToken { get; set; }
        private int IsAdmin { get; set; }
    }
}