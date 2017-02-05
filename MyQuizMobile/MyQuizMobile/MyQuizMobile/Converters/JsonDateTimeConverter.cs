using System;
using Newtonsoft.Json;

namespace MyQuizMobile.Converters {
    public class JsonDateTimeConverter : JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var text = value.ToString();

            writer.WritePropertyName("DateTime");
            writer.WriteValue(text);
            writer.Flush();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer) {
            var dataString = (string)reader.Value;
            if (dataString != null) {
                var datetime = DateTime.ParseExact(dataString, "HH:mm", null);
                return datetime;
            }

            return null;
        }

        public override bool CanConvert(Type objectType) { return true; }
    }

    public class JsonMultipleChoiceConverter : JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var text = (bool)value ? "multi" : "single";

            writer.WritePropertyName("MultipleChoice");
            writer.WriteValue(text);
            writer.Flush();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer) {
            return (string)reader.Value == "multi";
        }

        public override bool CanConvert(Type objectType) { return false; }
    }

    public class JsonIsCorrectConverter : JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var text = (bool)value ? "1" : "0";

            writer.WritePropertyName("IsCorrect");
            writer.WriteValue(text);
            writer.Flush();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer) {
            return (string)reader.Value == "1";
        }

        public override bool CanConvert(Type objectType) { return false; }
    }

    public class JsonIsAdminConverter : JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var text = (bool)value ? 1 : 0;

            writer.WritePropertyName("IsAdmin");
            writer.WriteValue(text);
            writer.Flush();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer) {
            return (int)reader.Value == 1;
        }

        public override bool CanConvert(Type objectType) { return false; }
    }
}