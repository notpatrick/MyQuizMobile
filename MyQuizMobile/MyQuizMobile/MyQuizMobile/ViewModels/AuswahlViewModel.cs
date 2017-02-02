using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyQuizMobile.DataModel;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class AuswahlViewModel : INotifyPropertyChanged {
        public AuswahlViewModel(MenuItem item) {
            ItemType = item.ItemType;
            switch (ItemType) {
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

        public ObservableCollection<MenuItem> AuswahlItemCollection { get; set; }
        public ItemType ItemType { get; set; }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e) {
            var item = e.SelectedItem as MenuItem;
            if (item == null) {
                return;
            }
            OnPicked(new MenuItemPickedEventArgs {Item = item});
        }

        #region event
        public event MenuItemPickedHanler PickDone;

        public delegate void MenuItemPickedHanler(object sender, MenuItemPickedEventArgs e);

        protected virtual void OnPicked(MenuItemPickedEventArgs e) { PickDone?.Invoke(this, e); }
        #endregion

        #region inotify
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class MenuItemPickedEventArgs : EventArgs {
        public MenuItem Item { get; set; }
    }

    public class AuswahlItem : MenuItem {
        //dummystuff
        public static ObservableCollection<MenuItem> VeranstaltungsDummy = new ObservableCollection<MenuItem> {
            new Group {
                DisplayText = "7235 - Projektmanagement",
                Id = 0,
                ItemType = ItemType.Veranstaltung,
                SingleTopics = new ObservableCollection<SingleTopic> {
                    new SingleTopic {DisplayText = "Lloyd"},
                    new SingleTopic {DisplayText = "Julian"},
                    new SingleTopic {DisplayText = "Philipp"},
                    new SingleTopic {DisplayText = "Jovan"},
                    new SingleTopic {DisplayText = "Patrick"}
                }
            },
            new Group {
                DisplayText = "5678 - Theoretische Informatik",
                Id = 1,
                ItemType = ItemType.Veranstaltung
            },
            new Group {
                DisplayText = "0798 - Projektarbeit",
                Id = 2,
                ItemType = ItemType.Veranstaltung,
                SingleTopics = new ObservableCollection<SingleTopic> {
                    new SingleTopic {DisplayText = "Lloyd"},
                    new SingleTopic {DisplayText = "Julian"},
                    new SingleTopic {DisplayText = "Philipp"},
                    new SingleTopic {DisplayText = "Jovan"},
                    new SingleTopic {DisplayText = "Patrick"}
                }
            },
            new Group {
                DisplayText = "3456 - Masterstudenten",
                Id = 3,
                ItemType = ItemType.Veranstaltung,
                SingleTopics = new ObservableCollection<SingleTopic> {
                    new SingleTopic {DisplayText = "Hans"},
                    new SingleTopic {DisplayText = "Franz"},
                    new SingleTopic {DisplayText = "Matthias"},
                    new SingleTopic {DisplayText = "Marco"},
                    new SingleTopic {DisplayText = "Samer"}
                }
            },
            new Group {
                DisplayText = "2349 - Erstsemester '17",
                Id = 4,
                ItemType = ItemType.Veranstaltung
            },
            new Group {
                DisplayText = "7914 - Grundlagen der Informatik",
                Id = 5,
                ItemType = ItemType.Veranstaltung
            },
            new Group {
                DisplayText = "5746 - Anwendungsentwicklung",
                Id = 6,
                ItemType = ItemType.Veranstaltung
            },
            new Group {
                DisplayText = "0225 - Edutainment und Lernspiele",
                Id = 7,
                ItemType = ItemType.Veranstaltung
            }
        };

        public static ObservableCollection<MenuItem> FragelistenDummy = new ObservableCollection<MenuItem> {
            new QuestionBlock {DisplayText = "Eine einzelne Frage wählen", Id = 0, ItemType = ItemType.Frageliste},
            new QuestionBlock {
                DisplayText =
                    "Ein extra langer Fragetext der nur dazu da ist um zu schauen ob in der App trotzdem alles Ordnungsgenäß angezeigt wird, haha!",
                Id = 1,
                ItemType = ItemType.Frageliste
            },
            new QuestionBlock {DisplayText = "Vorlesungsfeedback", Id = 2, ItemType = ItemType.Frageliste},
            new QuestionBlock {DisplayText = "Gastdozenteneindruck", Id = 3, ItemType = ItemType.Frageliste},
            new QuestionBlock {DisplayText = "XAML Quiz", Id = 4, ItemType = ItemType.Frageliste},
            new QuestionBlock {DisplayText = "Wetterumfrage", Id = 5, ItemType = ItemType.Frageliste}
        };

        public static ObservableCollection<MenuItem> FragenDummy = new ObservableCollection<MenuItem> {
            new Question {DisplayText = "Wie fandest du die Vorlesung?", Id = 0, ItemType = ItemType.Frage},
            new Question {
                DisplayText = "An welchem Tag soll die Prüfung stattfinden?",
                Id = 1,
                ItemType = ItemType.Frage
            },
            new Question {
                DisplayText =
                    "Ein extra langer Fragetext der nur dazu da ist um zu schauen ob in der App trotzdem alles Ordnungsgenäß angezeigt wird, haha!",
                Id = 2,
                ItemType = ItemType.Frage
            },
            new Question {DisplayText = "Wo geht morgens die Sonne auf?", Id = 3, ItemType = ItemType.Frage},
            new Question {DisplayText = "Wie geht es dir heute?", Id = 4, ItemType = ItemType.Frage},
            new Question {DisplayText = "Brauchst du eine Pause?", Id = 5, ItemType = ItemType.Frage}
        };

        public AuswahlItem() { DisplayText = "Default Entry"; }

        public AuswahlItem(MenuItem item) {
            Id = item.Id;
            DisplayText = item.DisplayText;
            ItemType = item.ItemType;
        }

        public override int Id { get; set; }
        public override string DisplayText { get; set; }
        public override ItemType ItemType { get; set; }
    }
}