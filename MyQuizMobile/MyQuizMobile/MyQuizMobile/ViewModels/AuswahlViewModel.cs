using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MyQuizMobile.DataModel;
using MYQuizMobile;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class AuswahlViewModel : INotifyPropertyChanged {
        private readonly List<MenuItem> _allItems = new List<MenuItem>();
        private readonly Networking _networking;
        private bool _isLoading;
        private string _searchString;
        public bool IsLoading {
            get { return _isLoading; }
            set {
                if (_isLoading != value) {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SearchString {
            get { return _searchString; }
            set {
                if (_searchString != value) {
                    _searchString = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<MenuItem> AuswahlItemCollection { get; set; }
        public ItemType ItemType { get; set; }

        public AuswahlViewModel(MenuItem item) {
            _networking = App.Networking;
            ItemType = item.ItemType;
            AuswahlItemCollection = new ObservableCollection<MenuItem>();
        }

        public async Task GetAll() {
            IsLoading = true;
            switch (ItemType) {
            case ItemType.Group:
                var resultGroups = await _networking.Get<List<Group>>("api/groups/");
                _allItems.Clear();
                AuswahlItemCollection.Clear();
                foreach (var g in resultGroups) {
                    _allItems.Add(g);
                    AuswahlItemCollection.Add(g);
                }
                break;
            case ItemType.QuestionBlock:
                var resultQuestionBlock = await _networking.Get<List<QuestionBlock>>("api/questionBlock/");
                _allItems.Clear();
                AuswahlItemCollection.Clear();
                foreach (var g in resultQuestionBlock) {
                    _allItems.Add(g);
                    AuswahlItemCollection.Add(g);
                }
                break;
            case ItemType.Question:
                var resultQuestion = await _networking.Get<List<Question>>("api/questions/");
                _allItems.Clear();
                AuswahlItemCollection.Clear();
                foreach (var g in resultQuestion) {
                    _allItems.Add(g);
                    AuswahlItemCollection.Add(g);
                }
                break;
            }
            IsLoading = false;
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e) {
            switch (ItemType) {
            case ItemType.Group:
                var itemGroup = e.SelectedItem as Group;
                if (itemGroup == null) {
                    return;
                }
                OnPicked(new MenuItemPickedEventArgs {Item = itemGroup});
                break;
            case ItemType.QuestionBlock:
                var itemQB = e.SelectedItem as QuestionBlock;
                if (itemQB == null) {
                    return;
                }
                OnPicked(new MenuItemPickedEventArgs {Item = itemQB});
                break;
            case ItemType.Question:
                var itemQ = e.SelectedItem as Question;
                if (itemQ == null) {
                    return;
                }
                OnPicked(new MenuItemPickedEventArgs {Item = itemQ});
                break;
            }
        }

        public void Filter() {
            IEnumerable<MenuItem> filtered;
            if (SearchString == string.Empty) {
                filtered = _allItems;
                AuswahlItemCollection.Clear();
                foreach (var g in filtered) {
                    AuswahlItemCollection.Add(g);
                }
                return;
            }
            filtered = _allItems.Where(x => x.DisplayText.ToLower().Contains(SearchString.ToLower()));
            AuswahlItemCollection.Clear();
            foreach (var g in filtered) {
                AuswahlItemCollection.Add(g);
            }
        }

        public async void listView_Refreshing(object sender, EventArgs e) { await GetAll(); }
        public void searchBar_TextChanged(object sender, TextChangedEventArgs e) { Filter(); }
        public async void OnAppearing(object sender, EventArgs e) { await GetAll(); }

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
                DisplayText = "Projektmanagement",
                EnterGroupPin = "6789",
                Id = 0,
                ItemType = ItemType.Group,
                SingleTopics = new ObservableCollection<SingleTopic> {
                    new SingleTopic {DisplayText = "Lloyd"},
                    new SingleTopic {DisplayText = "Julian"},
                    new SingleTopic {DisplayText = "Philipp"},
                    new SingleTopic {DisplayText = "Jovan"},
                    new SingleTopic {DisplayText = "Patrick"}
                }
            },
            new Group {
                DisplayText = "Theoretische Informatik",
                EnterGroupPin = "1234",
                Id = 1,
                ItemType = ItemType.Group
            },
            new Group {
                DisplayText = "Projektarbeit",
                EnterGroupPin = "4567",
                Id = 2,
                ItemType = ItemType.Group,
                SingleTopics = new ObservableCollection<SingleTopic> {
                    new SingleTopic {DisplayText = "Lloyd"},
                    new SingleTopic {DisplayText = "Julian"},
                    new SingleTopic {DisplayText = "Philipp"},
                    new SingleTopic {DisplayText = "Jovan"},
                    new SingleTopic {DisplayText = "Patrick"}
                }
            },
            new Group {
                DisplayText = "Masterstudenten",
                EnterGroupPin = "3456",
                Id = 3,
                ItemType = ItemType.Group,
                SingleTopics = new ObservableCollection<SingleTopic> {
                    new SingleTopic {DisplayText = "Hans"},
                    new SingleTopic {DisplayText = "Franz"},
                    new SingleTopic {DisplayText = "Matthias"},
                    new SingleTopic {DisplayText = "Marco"},
                    new SingleTopic {DisplayText = "Samer"}
                }
            },
            new Group {
                DisplayText = "Erstsemester '17",
                EnterGroupPin = "2349",
                Id = 4,
                ItemType = ItemType.Group
            },
            new Group {
                DisplayText = "Grundlagen der Informatik",
                EnterGroupPin = "7914",
                Id = 5,
                ItemType = ItemType.Group
            },
            new Group {
                DisplayText = "Anwendungsentwicklung",
                EnterGroupPin = "5746",
                Id = 6,
                ItemType = ItemType.Group
            },
            new Group {
                DisplayText = "Edutainment und Lernspiele",
                EnterGroupPin = "0225",
                Id = 7,
                ItemType = ItemType.Group
            }
        };

        public static ObservableCollection<MenuItem> FragelistenDummy = new ObservableCollection<MenuItem> {
            new QuestionBlock {DisplayText = "Eine einzelne Frage wählen", Id = 0, ItemType = ItemType.QuestionBlock},
            new QuestionBlock {
                DisplayText =
                    "Ein extra langer Fragetext der nur dazu da ist um zu schauen ob in der App trotzdem alles Ordnungsgenäß angezeigt wird, haha!",
                Id = 1,
                ItemType = ItemType.QuestionBlock
            },
            new QuestionBlock {DisplayText = "Vorlesungsfeedback", Id = 2, ItemType = ItemType.QuestionBlock},
            new QuestionBlock {DisplayText = "Gastdozenteneindruck", Id = 3, ItemType = ItemType.QuestionBlock},
            new QuestionBlock {DisplayText = "XAML Quiz", Id = 4, ItemType = ItemType.QuestionBlock},
            new QuestionBlock {DisplayText = "Wetterumfrage", Id = 5, ItemType = ItemType.QuestionBlock}
        };

        public static ObservableCollection<MenuItem> FragenDummy = new ObservableCollection<MenuItem> {
            new Question {DisplayText = "Wie fandest du die Vorlesung?", Id = 0, ItemType = ItemType.Question},
            new Question {
                DisplayText = "An welchem Tag soll die Prüfung stattfinden?",
                Id = 1,
                ItemType = ItemType.Question
            },
            new Question {
                DisplayText =
                    "Ein extra langer Fragetext der nur dazu da ist um zu schauen ob in der App trotzdem alles Ordnungsgenäß angezeigt wird, haha!",
                Id = 2,
                ItemType = ItemType.Question
            },
            new Question {DisplayText = "Wo geht morgens die Sonne auf?", Id = 3, ItemType = ItemType.Question},
            new Question {DisplayText = "Wie geht es dir heute?", Id = 4, ItemType = ItemType.Question},
            new Question {DisplayText = "Brauchst du eine Pause?", Id = 5, ItemType = ItemType.Question}
        };

        public override int Id { get; set; }
        public override string DisplayText { get; set; }
        public override ItemType ItemType { get; set; }

        public AuswahlItem() { DisplayText = "Default Entry"; }

        public AuswahlItem(MenuItem item) {
            Id = item.Id;
            DisplayText = item.DisplayText;
            ItemType = item.ItemType;
        }
    }
}