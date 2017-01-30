namespace MyQuizMobile
{
    public interface IMenuItem
    {
        int Id { get; set; }
        string DisplayText { get; set; }
        ItemType ItemType { get; set; }
    }

    public enum ItemType
    {
        Veranstaltung,
        Frageliste,
        Frage,
        Antwort,
        Person
    }
}
