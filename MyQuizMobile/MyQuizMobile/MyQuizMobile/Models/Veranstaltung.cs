using System.Collections.Generic;

namespace MyQuizMobile
{
    public class Veranstaltung : IMenuItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public ItemType ItemType { get; set; }
        public string Name { get; set; }
        public List<Person> Personen { get;set; }

        public Veranstaltung()
        {
            DisplayText = $"{Name}";
            ItemType = ItemType.Person;
        }
    }
}
