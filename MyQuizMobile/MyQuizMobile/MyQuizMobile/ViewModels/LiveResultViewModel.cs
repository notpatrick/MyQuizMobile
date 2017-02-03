using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MyQuizMobile.DataModel;
using MyQuizMobile.Helpers;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace MyQuizMobile {
    public class LiveResultViewModel : INotifyPropertyChanged {
        private const string SendenText = "Absenden";
        private const string BeendenText = "Umfrage beenden";
        private const string GesendetText = "Bereits gesendet";
        private readonly int _initialTime;
        private bool _abstimmungFertig;
        private string _buttonText;
        private bool _canEdit;
        private bool _canSend;
        private SingleTopic _currentSingleTopic;
        private bool _isPersonenbezogen;
        private int _timeInSeconds;

        private Timer _timer;

        public ObservableCollection<MenuItem> ResultCollection { get; set; }
        public int TimeInSeconds {
            get { return _timeInSeconds; }
            set {
                _timeInSeconds = value;
                OnPropertyChanged();
            }
        }
        public bool CanSend {
            get { return _canSend; }
            set {
                _canSend = value;
                OnPropertyChanged();
            }
        }
        public bool CanEdit {
            get { return _canEdit; }
            set {
                _canEdit = value;
                OnPropertyChanged();
            }
        }
        public SingleTopic CurrentSingleTopic {
            get { return _currentSingleTopic; }
            set {
                _currentSingleTopic = value;
                OnPropertyChanged();
            }
        }
        public bool IsPersonenbezogen {
            get { return _isPersonenbezogen; }
            set {
                _isPersonenbezogen = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<SingleTopic> SingleTopics { get; set; }
        public string ButtonText {
            get { return _buttonText; }
            set {
                _buttonText = value;
                OnPropertyChanged();
            }
        }

        public LiveResultViewModel(AbstimmungStartenViewModel asvm) {
            CanSend = true;
            CanEdit = true;
            ButtonText = SendenText;
            TimeInSeconds = asvm.TimeInSeconds;
            _initialTime = asvm.TimeInSeconds;
            IsPersonenbezogen = asvm.IsPersonenbezogen;
            ResultCollection = ResultItem.ResultsDummy;
            if (IsPersonenbezogen) {
                SingleTopics = ((Group)asvm.OptionCollection[0]).SingleTopics;
                CurrentSingleTopic = SingleTopics.FirstOrDefault();
            }
        }

        public async void weiterButton_Clicked(object sender, EventArgs e) {
            if (!_abstimmungFertig) {
                CanSend = false;
                CanEdit = false;
                _timer = new Timer(TimerOnElapsed, null, 0, 1000);
                // TODO: Open websocket connection here to start
            } else {
                await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync();
            }
        }

        private void TimerOnElapsed(object sender) {
            if (TimeInSeconds > 0) {
                TimeInSeconds -= 1;
            } else {
                _timer.Cancel();
                CanSend = true;
                CanEdit = true;
                if (IsPersonenbezogen) {
                    CurrentSingleTopic.UmfrageDone = true;
                    if (SingleTopics.Any(singleTopic => !singleTopic.UmfrageDone)) {
                        TimeInSeconds = _initialTime;
                        var index = SingleTopics.IndexOf(CurrentSingleTopic);
                        if (index < SingleTopics.Count - 1 && !SingleTopics.ElementAt(index + 1).UmfrageDone) {
                            CurrentSingleTopic = SingleTopics.ElementAt(index + 1);
                        } else {
                            CurrentSingleTopic = SingleTopics.FirstOrDefault(x => x.UmfrageDone == false);
                        }
                    }
                }

                CheckIfAbstimmungFertig();
            }
        }

        private void CheckIfAbstimmungFertig() {
            if (SingleTopics != null && SingleTopics.Any(singleTopic => !singleTopic.UmfrageDone)) {
                _abstimmungFertig = false;
                if (CurrentSingleTopic.UmfrageDone) {
                    CanSend = false;
                    ButtonText = GesendetText;
                } else {
                    CanSend = true;
                    ButtonText = SendenText;
                }
                return;
            }
            _abstimmungFertig = true;
            ButtonText = BeendenText;
        }

        public void timeEntry_OnFocused(object sender, FocusEventArgs e) {
            var entry = e.VisualElement as Entry;
            if (entry == null) {
                return;
            }
            // TODO: not mvvm style
            entry.Text = "";
        }

        public void timeEntry_OnUnfocused(object sender, FocusEventArgs e) {
            var entry = e.VisualElement as Entry;
            if (entry == null) {
                return;
            }
            int newTime;
            if (int.TryParse(entry.Text, out newTime)) {
                if (newTime < 1) {
                    newTime = 1;
                } else if (newTime > 9999) {
                    newTime = 9999;
                }
                TimeInSeconds = newTime;
            } else {
                TimeInSeconds = _initialTime;
            }
        }

        public bool OnBackButtonPressed(ContentPage page) {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await page.DisplayAlert("Achtung!",
                                                     "Wollen Sie die aktuelle Umfrage wirklich vorzeitig beenden? Ergebnisse können dann unvollständig sein!",
                                                     "Umfrage Beenden", "Zurück");
                if (result) {
                    await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync();
                }
            });

            return true;
        }

        public void listView_ItemTapped(object sender, ItemTappedEventArgs e) {
            if (e.Item == null) {
                return;
            }
            ((ListView)sender).SelectedItem = null;
        }

        public void personPicker_ItemSelected(object sender, SelectedItemChangedEventArgs e) {
            CheckIfAbstimmungFertig();
        }

        #region notify
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class ResultItem : MenuItem {
        public static ObservableCollection<MenuItem> ResultsDummy = new ObservableCollection<MenuItem> {
            new ResultItem {
                DisplayText = "Wie fandest du die Vorlesung?",
                Id = 0,
                ItemType = ItemType.Question,
                AnswerCount = 11,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "An welchem Tag soll die Prüfung stattfinden?",
                Id = 1,
                ItemType = ItemType.Question,
                AnswerCount = 7,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText =
                    "Ein extra langer Fragetext der nur dazu da ist um zu schauen ob in der App trotzdem alles Ordnungsgenäß angezeigt wird, haha!",
                Id = 2,
                ItemType = ItemType.Question,
                AnswerCount = 15,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Wo geht morgens die Sonne auf?",
                Id = 3,
                ItemType = ItemType.Question,
                AnswerCount = 21,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Wie geht es dir heute?",
                Id = 4,
                ItemType = ItemType.Question,
                AnswerCount = 11,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Brauchst du eine Pause?",
                Id = 5,
                ItemType = ItemType.Question,
                AnswerCount = 4,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Wie fandest du die Vorlesung?",
                Id = 0,
                ItemType = ItemType.Question,
                AnswerCount = 11,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "An welchem Tag soll die Prüfung stattfinden?",
                Id = 1,
                ItemType = ItemType.Question,
                AnswerCount = 7,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText =
                    "Ein extra langer Fragetext der nur dazu da ist um zu schauen ob in der App trotzdem alles Ordnungsgenäß angezeigt wird, haha!",
                Id = 2,
                ItemType = ItemType.Question,
                AnswerCount = 15,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Wo geht morgens die Sonne auf?",
                Id = 3,
                ItemType = ItemType.Question,
                AnswerCount = 21,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Wie geht es dir heute?",
                Id = 4,
                ItemType = ItemType.Question,
                AnswerCount = 11,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Brauchst du eine Pause?",
                Id = 5,
                ItemType = ItemType.Question,
                AnswerCount = 4,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Wie fandest du die Vorlesung?",
                Id = 0,
                ItemType = ItemType.Question,
                AnswerCount = 11,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "An welchem Tag soll die Prüfung stattfinden?",
                Id = 1,
                ItemType = ItemType.Question,
                AnswerCount = 7,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText =
                    "Ein extra langer Fragetext der nur dazu da ist um zu schauen ob in der App trotzdem alles Ordnungsgenäß angezeigt wird, haha!",
                Id = 2,
                ItemType = ItemType.Question,
                AnswerCount = 15,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Wo geht morgens die Sonne auf?",
                Id = 3,
                ItemType = ItemType.Question,
                AnswerCount = 21,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Wie geht es dir heute?",
                Id = 4,
                ItemType = ItemType.Question,
                AnswerCount = 11,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Brauchst du eine Pause?",
                Id = 5,
                ItemType = ItemType.Question,
                AnswerCount = 4,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Wie fandest du die Vorlesung?",
                Id = 0,
                ItemType = ItemType.Question,
                AnswerCount = 11,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "An welchem Tag soll die Prüfung stattfinden?",
                Id = 1,
                ItemType = ItemType.Question,
                AnswerCount = 7,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText =
                    "Ein extra langer Fragetext der nur dazu da ist um zu schauen ob in der App trotzdem alles Ordnungsgenäß angezeigt wird, haha!",
                Id = 2,
                ItemType = ItemType.Question,
                AnswerCount = 15,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Wo geht morgens die Sonne auf?",
                Id = 3,
                ItemType = ItemType.Question,
                AnswerCount = 21,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Wie geht es dir heute?",
                Id = 4,
                ItemType = ItemType.Question,
                AnswerCount = 11,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            },
            new ResultItem {
                DisplayText = "Brauchst du eine Pause?",
                Id = 5,
                ItemType = ItemType.Question,
                AnswerCount = 4,
                Antworten = new ObservableCollection<AnswerOption> {
                    new AnswerOption {
                        DisplayText = "AnswerOption a",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Vielleicht b",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Es ist doch c",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    },
                    new AnswerOption {
                        DisplayText = "Aber d ist immer richtig",
                        Id = 0,
                        ItemType = ItemType.AnswerOption,
                        IsCorrect = false
                    }
                }
            }
        };

        public override int Id { get; set; }
        public override string DisplayText { get; set; }
        public override ItemType ItemType { get; set; }
        public ObservableCollection<AnswerOption> Antworten { get; set; }
        public int AnswerCount { get; set; }

        public ResultItem() { ItemType = ItemType.Question; }
    }
}