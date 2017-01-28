using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Android.Locations;
using Xamarin.Forms;

namespace MyQuizMobile
{
    public class EntrySelectPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EntrySelectItem> EntrySelectItemCollection { get; set; }
        private DetailMain _detailMain;
        public string Type
        {
            get { return _type; } 
            set
            {
                OnPropertyChanged("Type");
                _type = value;
            }
        }
        private string _type;
        public event EventHandler PickDone ;
        protected virtual void OnPicked(EventArgs e)
        {
            PickDone?.Invoke(this, e);
        }
        public EntrySelectPageViewModel(string type, DetailMain detailmain)
        {
            _type = type;
            _detailMain = detailmain;
            PickDone += _detailMain.SetEntryVeranstaltung;
            switch (type)
            {
                case "Veranstaltungen":
                    EntrySelectItemCollection = EntrySelectItem.Dummy;
                    break;
                default:
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as EntrySelectItem;

            if (item == null) return;
            OnPicked(new StringEventArg {Name = item.Title});

        }
    }

    public class StringEventArg : EventArgs
    {
        public string Name { get; set; }
    }

    public class EntrySelectItem
    {
        public string Title { get; set; }
        public EntrySelectItem()
        {
            Title = "Default Entry";
        }

        public EntrySelectItem(string title)
        {
            Title = title;
        }

        public static ObservableCollection<EntrySelectItem> Dummy = new ObservableCollection<EntrySelectItem>
                        {
                            new EntrySelectItem("7235 - Projektmanagement"),
                            new EntrySelectItem("5678 - Theoretische Informatik"),
                            new EntrySelectItem("0798 - Projektarbeit"),
                            new EntrySelectItem("3456 - Masterstudenten"),
                            new EntrySelectItem("2349 - Erstsemester '17"),
                            new EntrySelectItem("7914 - Grundlagen der Informatik"),
                            new EntrySelectItem("5746 - Anwendungsentwicklung"),
                            new EntrySelectItem("0225 - Edutainment und Lernspiele"),
                            new EntrySelectItem("7235 - Projektmanagement"),
                            new EntrySelectItem("5678 - Theoretische Informatik"),
                            new EntrySelectItem("0798 - Projektarbeit"),
                            new EntrySelectItem("3456 - Masterstudenten"),
                            new EntrySelectItem("2349 - Erstsemester '17"),
                            new EntrySelectItem("7914 - Grundlagen der Informatik"),
                            new EntrySelectItem("5746 - Anwendungsentwicklung"),
                            new EntrySelectItem("0225 - Edutainment und Lernspiele"),
                        };
    }
}
