using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MyQuizMobile
{
    public class AuswahlViewModel : INotifyPropertyChanged
    {
        public AuswahlViewModel(IMenuItem item)
        {
            ItemType = item.ItemType;
            switch (ItemType)
            {
                case ItemType.Veranstaltung:
                    AuswahlItemCollection = AuswahlItem.VeranstaltungsDummy;
                    break;
                case ItemType.Frageliste:
                    AuswahlItemCollection = AuswahlItem.FragelistenDummy;
                    break;
                case ItemType.Frage:
                    AuswahlItemCollection = AuswahlItem.FragenDummy;
                    break;
                // TODO: determine if these can be done here or need a seperate page
                case ItemType.Antwort:
                    break;
                case ItemType.Person:
                    break;
                default:
                    break;
            }
        }

        public ObservableCollection<IMenuItem> AuswahlItemCollection { get; set; }
        public ItemType ItemType { get; set; }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as IMenuItem;
            if (item == null) return;
            OnPicked(new MenuItemPickedEventArgs {Item = item});
        }

        #region event

        public event MenuItemPickedHanler PickDone;

        public delegate void MenuItemPickedHanler(object sender, MenuItemPickedEventArgs e);

        protected virtual void OnPicked(MenuItemPickedEventArgs e)
        {
            PickDone?.Invoke(this, e);
        }

        #endregion

        #region inotify

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class MenuItemPickedEventArgs : EventArgs
    {
        public IMenuItem Item { get; set; }
    }

    public class AuswahlItem : IMenuItem
    {
        //dummystuff
        public static ObservableCollection<IMenuItem> VeranstaltungsDummy = new ObservableCollection<IMenuItem>
        {
            new Veranstaltung
            {
                DisplayText = "7235 - Projektmanagement",
                Id = 0,
                ItemType = ItemType.Veranstaltung,
                Personen = new ObservableCollection<Person>
                {
                    new Person {DisplayText = "Lloyd"},
                    new Person {DisplayText = "Julian"},
                    new Person {DisplayText = "Philipp"},
                    new Person {DisplayText = "Jovan"},
                    new Person {DisplayText = "Patrick"}
                }
            },
            new Veranstaltung
            {
                DisplayText = "5678 - Theoretische Informatik",
                Id = 1,
                ItemType = ItemType.Veranstaltung
            },
            new Veranstaltung
            {
                DisplayText = "0798 - Projektarbeit",
                Id = 2,
                ItemType = ItemType.Veranstaltung,
                Personen = new ObservableCollection<Person>
                {
                    new Person {DisplayText = "Lloyd"},
                    new Person {DisplayText = "Julian"},
                    new Person {DisplayText = "Philipp"},
                    new Person {DisplayText = "Jovan"},
                    new Person {DisplayText = "Patrick"}
                }
            },
            new Veranstaltung
            {
                DisplayText = "3456 - Masterstudenten",
                Id = 3,
                ItemType = ItemType.Veranstaltung,
                Personen = new ObservableCollection<Person>
                {
                    new Person {DisplayText = "Hans"},
                    new Person {DisplayText = "Franz"},
                    new Person {DisplayText = "Matthias"},
                    new Person {DisplayText = "Marco"},
                    new Person {DisplayText = "Samer"}
                }
            },
            new Veranstaltung
            {
                DisplayText = "2349 - Erstsemester '17",
                Id = 4,
                ItemType = ItemType.Veranstaltung,
                Personen = new ObservableCollection<Person>
                {
                    new Person {DisplayText = "Dominik"},
                    new Person {DisplayText = "Julian"},
                    new Person {DisplayText = "Philipp"},
                    new Person {DisplayText = "Jovan"},
                    new Person {DisplayText = "Patrick"}
                }
            },
            new Veranstaltung
            {
                DisplayText = "7914 - Grundlagen der Informatik",
                Id = 5,
                ItemType = ItemType.Veranstaltung,
                Personen = new ObservableCollection<Person>
                {
                    new Person {DisplayText = "Lloyd"},
                    new Person {DisplayText = "Julian"},
                    new Person {DisplayText = "Philipp"},
                    new Person {DisplayText = "Jovan"},
                    new Person {DisplayText = "Patrick"}
                }
            },
            new Veranstaltung
            {
                DisplayText = "5746 - Anwendungsentwicklung",
                Id = 6,
                ItemType = ItemType.Veranstaltung,
                Personen = new ObservableCollection<Person>
                {
                    new Person {DisplayText = "Lloyd"},
                    new Person {DisplayText = "Julian"},
                    new Person {DisplayText = "Philipp"},
                    new Person {DisplayText = "Jovan"},
                    new Person {DisplayText = "Patrick"}
                }
            },
            new Veranstaltung
            {
                DisplayText = "0225 - Edutainment und Lernspiele",
                Id = 7,
                ItemType = ItemType.Veranstaltung,
                Personen = new ObservableCollection<Person>
                {
                    new Person {DisplayText = "Lloyd"},
                    new Person {DisplayText = "Julian"},
                    new Person {DisplayText = "Philipp"},
                    new Person {DisplayText = "Jovan"},
                    new Person {DisplayText = "Patrick"}
                }
            }
        };

        public static ObservableCollection<IMenuItem> FragelistenDummy = new ObservableCollection<IMenuItem>
        {
            new FrageListe {DisplayText = "Eine einzelne Frage wählen", Id = 0, ItemType = ItemType.Frageliste},
            new FrageListe
            {
                DisplayText =
                    "Ein extra langer Fragetext der nur dazu da ist um zu schauen ob in der App trotzdem alles Ordnungsgenäß angezeigt wird, haha!",
                Id = 1,
                ItemType = ItemType.Frageliste
            },
            new FrageListe {DisplayText = "Vorlesungsfeedback", Id = 2, ItemType = ItemType.Frageliste},
            new FrageListe {DisplayText = "Gastdozenteneindruck", Id = 3, ItemType = ItemType.Frageliste},
            new FrageListe {DisplayText = "XAML Quiz", Id = 4, ItemType = ItemType.Frageliste},
            new FrageListe {DisplayText = "Wetterumfrage", Id = 5, ItemType = ItemType.Frageliste}
        };

        public static ObservableCollection<IMenuItem> FragenDummy = new ObservableCollection<IMenuItem>
        {
            new Frage {DisplayText = "Wie fandest du die Vorlesung?", Id = 0, ItemType = ItemType.Frage},
            new Frage {DisplayText = "An welchem Tag soll die Prüfung stattfinden?", Id = 1, ItemType = ItemType.Frage},
            new Frage
            {
                DisplayText =
                    "Ein extra langer Fragetext der nur dazu da ist um zu schauen ob in der App trotzdem alles Ordnungsgenäß angezeigt wird, haha!",
                Id = 2,
                ItemType = ItemType.Frage
            },
            new Frage {DisplayText = "Wo geht morgens die Sonne auf?", Id = 3, ItemType = ItemType.Frage},
            new Frage {DisplayText = "Wie geht es dir heute?", Id = 4, ItemType = ItemType.Frage},
            new Frage {DisplayText = "Brauchst du eine Pause?", Id = 5, ItemType = ItemType.Frage}
        };

        public AuswahlItem()
        {
            DisplayText = "Default Entry";
        }

        public AuswahlItem(IMenuItem item)
        {
            Id = item.Id;
            DisplayText = item.DisplayText;
            ItemType = item.ItemType;
        }

        public int Id { get; set; }
        public string DisplayText { get; set; }
        public ItemType ItemType { get; set; }
    }
}