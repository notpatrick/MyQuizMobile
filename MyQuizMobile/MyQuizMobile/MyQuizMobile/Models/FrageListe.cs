using System.Collections.Generic;

namespace MyQuizMobile
{
    public class FrageListe : IMenuItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public ItemType ItemType { get; set; }
        public string Name { get; set; }
        public List<Frage> Fragen { get; set; }

        public FrageListe()
        {
            DisplayText = $"{Name}";
            ItemType = ItemType.Frageliste;
        }

    }
}
