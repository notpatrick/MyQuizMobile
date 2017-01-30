using System.Collections.Generic;

namespace MyQuizMobile
{
    public class Frage : IMenuItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public ItemType ItemType { get; set; }
        public string Text { get; set; }
        public List<Antwort> Antworten { get; set; }

        public Frage()
        {
            DisplayText = $"{Text}";
            ItemType = ItemType.Frage;
        }
    }
}
