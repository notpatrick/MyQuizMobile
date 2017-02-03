using System;
using System.Collections.ObjectModel;
using System.Linq;
using MyQuizMobile.DataModel;
using MyQuizMobile.Helpers;
using PostSharp.Patterns.Model;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class LiveResultViewModel {
        private const string SendenText = "Absenden";
        private const string BeendenText = "Umfrage beenden";
        private const string GesendetText = "Bereits gesendet";
        private readonly int _initialTime;
        private bool _abstimmungFertig;
        private Timer _timer;

        public ObservableCollection<MenuItem> ResultCollection { get; set; }
        public int TimeInSeconds { get; set; }
        public bool CanSend { get; set; }
        public bool CanEdit { get; set; }
        public SingleTopic CurrentSingleTopic { get; set; }
        public bool IsPersonenbezogen { get; set; }
        public ObservableCollection<SingleTopic> SingleTopics { get; set; }
        public string ButtonText { get; set; }

        public LiveResultViewModel(AbstimmungStartenViewModel asvm) {
            CanSend = true;
            CanEdit = true;
            ButtonText = SendenText;
            TimeInSeconds = asvm.TimeInSeconds;
            _initialTime = asvm.TimeInSeconds;
            IsPersonenbezogen = asvm.IsPersonenbezogen;
            // TODO: REPLACE WITH REMOTE RESULTS
            ResultCollection = new ObservableCollection<MenuItem>();
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
    }
}