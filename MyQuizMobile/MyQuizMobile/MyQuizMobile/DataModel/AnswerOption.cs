using MyQuizMobile.Converters;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public partial class AnswerOption {
        public override long Id { get; set; }
        public string Text { get; set; }
        [JsonConverter(typeof(JsonIsCorrectConverter))]
        public bool IsCorrect { get; set; }
    }
}