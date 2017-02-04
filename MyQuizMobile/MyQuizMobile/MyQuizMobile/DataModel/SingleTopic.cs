using MyQuizMobile.Converters;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public partial class SingleTopic {
        public override int Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public string DateTime { get; set; }
    }
}