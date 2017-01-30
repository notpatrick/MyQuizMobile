namespace MyQuizMobile
{
    public class Antwort : IMenuItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public ItemType ItemType { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public Antwort()
        {
            DisplayText = $"{Text}";
            ItemType = ItemType.Antwort;
        }
    }
}
