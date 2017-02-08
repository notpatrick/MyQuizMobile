using System;
using Newtonsoft.Json;

namespace MyQuizMobile.Converters {
    public class JsonDateTimeConverter : JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { serializer.Serialize(writer, value); }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (reader.Value == null) {
                return null;
            }
            var dataString = (string)reader.Value;
            if (dataString == null) {
                return null;
            }
            var datetime = DateTime.ParseExact(dataString, "HH:mm", null);
            return datetime;
        }

        public override bool CanConvert(Type objectType) { return true; }
    }
}