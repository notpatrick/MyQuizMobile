using MyQuizMobile.Converters;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public partial class Device {
        public override int Id { get; set; }
        public string PushUpToken { get; set; }
        [JsonConverter(typeof(JsonIsAdminConverter))]
        public bool IsAdmin { get; set; }
    }
}