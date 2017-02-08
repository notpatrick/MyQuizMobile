namespace MyQuizMobile.DataModel {
    public partial class Device {
        public override long Id { get; set; }
        public string PushUpToken { get; set; }
        public long IsAdmin { get; set; }
    }
}