using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace MyQuizMobile
{
    public class Antwort : IMenuItem, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public ItemType ItemType { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        private int _count;
        public int Count { get { return _count; } set { _count = value; OnPropertyChanged("Count"); } }

        public Antwort()
        {
            DisplayText = $"{Text}";
            ItemType = ItemType.Antwort;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
