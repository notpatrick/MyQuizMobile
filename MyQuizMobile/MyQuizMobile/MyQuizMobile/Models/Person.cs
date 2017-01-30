namespace MyQuizMobile
{
    public class Person : IMenuItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public ItemType ItemType { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }

        public Person()
        {
            DisplayText = $"{Nachname}, {Vorname}";
            ItemType = ItemType.Person;
        }
    }
}
