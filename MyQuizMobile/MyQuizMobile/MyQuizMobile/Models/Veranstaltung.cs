using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyQuizMobile
{
    public class Veranstaltung : IMenuItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public ItemType ItemType { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Person> Personen { get;set; }

        public Veranstaltung()
        {
            DisplayText = $"{Name}";
            ItemType = ItemType.Person;
        }
    }
}
