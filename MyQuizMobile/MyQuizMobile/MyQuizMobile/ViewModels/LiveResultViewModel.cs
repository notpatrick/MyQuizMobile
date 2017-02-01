using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using MyQuizMobile.Helpers;
using Xamarin.Forms;

namespace MyQuizMobile
{
    public class LiveResultViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IMenuItem> ResultCollection { get; set; }
        private int _timeInSeconds;
        public int TimeInSeconds
        {
            get { return _timeInSeconds; }
            set
            {
                _timeInSeconds = value;
                OnPropertyChanged("TimeInSeconds");
            }
        }
        private bool _canSend;
        public bool CanSend
        {
            get { return _canSend; }
            set
            {
                _canSend = value;
                OnPropertyChanged("CanSend");
            }
        }
        private Person _currentPerson;
        public Person CurrentPerson
        {
            get { return _currentPerson; }
            set
            {
                _currentPerson = value;
                OnPropertyChanged("CurrentPerson");
            }
        }
        private bool _isPersonenbezogen;
        public bool IsPersonenbezogen
        {
            get { return _isPersonenbezogen; }
            set
            {
                _isPersonenbezogen = value;
                OnPropertyChanged("IsPersonenbezogen");
            }
        }
        public ObservableCollection<Person> Persons { get; set; }

        private Timer _timer;
        public LiveResultViewModel(AbstimmungStartenViewModel asvm)
        {
            CanSend = true;
            TimeInSeconds = asvm.TimeInSeconds;
            IsPersonenbezogen = asvm.IsPersonenbezogen;
            ResultCollection = ResultItem.ResultsDummy;
            if (IsPersonenbezogen)
            {
                Persons = ((Veranstaltung)asvm.OptionCollection[0]).Personen;
                CurrentPerson = Persons[0];
            }
        }

        #region notify
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
#endregion

        public void weiterButton_Clicked(object sender, EventArgs e)
        {
            CanSend = !CanSend;
            _timer = new Timer(new TimerCallback(TimerOnElapsed), null, 0, 1000);
        }

        private void TimerOnElapsed(object sender)
        {
            if (TimeInSeconds > 0)
            {
                TimeInSeconds -= 1;
            }
            else
            {
                _timer.Cancel();
                CanSend = !CanSend;
                TimeInSeconds = 30;
                if (IsPersonenbezogen)
                {
                    var curindex = Persons.IndexOf(CurrentPerson);
                    CurrentPerson = curindex < Persons.Count ? Persons[(curindex + 1)] : Persons[0];
                }
            }
        }

        public void TimeEntryOnFocused(object sender, FocusEventArgs focusEventArgs)
        {
            var entry = focusEventArgs.VisualElement as Entry;
            if (entry != null) entry.Text = entry.Text.Replace("s", "");
        }

        public void TimeEntryOnUnfocused(object sender, FocusEventArgs focusEventArgs)
        {
            var entry = focusEventArgs.VisualElement as Entry;
            if (entry != null) entry.Text += "s";
        }
    }

    public class ResultItem : IMenuItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public ItemType ItemType { get; set; }
        public ObservableCollection<Antwort> Antworten { get; set; }

        public ResultItem()
        {
            ItemType = ItemType.Frage;
        }

        public static ObservableCollection<IMenuItem> ResultsDummy = new ObservableCollection<IMenuItem>
                {
                new ResultItem() { DisplayText = "Wie fandest du die Vorlesung?", Id = 0, ItemType = ItemType.Frage, Antworten = new  ObservableCollection<Antwort>() {
                    new Antwort() {DisplayText = "Antwort a", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Vielleicht b", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Es ist doch c", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Aber d ist immer richtig", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                }},
                new ResultItem() { DisplayText = "An welchem Tag soll die Prüfung stattfinden?", Id = 1, ItemType = ItemType.Frage,
                Antworten = new ObservableCollection<Antwort>()
                {
                    new Antwort() {DisplayText = "Antwort a", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Vielleicht b", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Es ist doch c", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Aber d ist immer richtig", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                }},
                            new ResultItem() { DisplayText = "Ein extra langer Fragetext der nur dazu da ist um zu schauen ob in der App trotzdem alles Ordnungsgenäß angezeigt wird, haha!", Id = 2, ItemType = ItemType.Frage,
                Antworten = new ObservableCollection<Antwort>()
                {
                    new Antwort() {DisplayText = "Antwort a", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Vielleicht b", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Es ist doch c", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Aber d ist immer richtig", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                }},
                            new ResultItem() { DisplayText = "Wo geht morgens die Sonne auf?", Id = 3, ItemType = ItemType.Frage,
                Antworten = new ObservableCollection<Antwort>()
                {
                    new Antwort() {DisplayText = "Antwort a", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Vielleicht b", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Es ist doch c", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Aber d ist immer richtig", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                }},
                            new ResultItem() { DisplayText = "Wie geht es dir heute?", Id = 4, ItemType = ItemType.Frage,
                Antworten = new ObservableCollection<Antwort>()
                {
                    new Antwort() {DisplayText = "Antwort a", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Vielleicht b", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Es ist doch c", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Aber d ist immer richtig", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                }},
                            new ResultItem() { DisplayText = "Brauchst du eine Pause?",Id = 5, ItemType = ItemType.Frage,
                Antworten = new ObservableCollection<Antwort>()
                {
                    new Antwort() {DisplayText = "Antwort a", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Vielleicht b", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Es ist doch c", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                    new Antwort() {DisplayText = "Aber d ist immer richtig", Id = 0, ItemType = ItemType.Antwort, IsCorrect = false, Count = 0},
                }}
                        };
    }
}
