using Newtonsoft.Json;
using NLog.Layouts;

namespace MyQuizMobile.DataModel {
    public partial class SingleTopic {
        public override long Id { get; set; }
        public string Name { get; set; }
        public string DateTime { get; set; }
    }
}