using Newtonsoft.Json;

namespace MyQuizMobile {
    public abstract class MenuItem {
        public abstract int Id { get; set; }
        [JsonIgnore]
        public abstract string DisplayText { get; set; }
        [JsonIgnore]
        public abstract ItemType ItemType { get; set; }
    }

    public enum ItemType {
        Veranstaltung,
        Frageliste,
        Frage,
        Antwort,
        Person
    }
}