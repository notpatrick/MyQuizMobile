using System;
using System.Collections.ObjectModel;
using System.Linq;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class VotingResultLiveViewModel {
        private const string TextSend = "Absenden";
        private const string TextExit = "Umfrage beenden";
        private const string TextAlreadySent = "Bereits gesendet";
        private readonly int _initialTime;
        private bool _voteFinished;

        public ObservableCollection<Item> ResultCollection { get; set; }
        public int TimeInSeconds { get; set; }
        public bool CanSend { get; set; }
        public bool CanEdit { get; set; }
        public SingleTopic CurrentSingleTopic { get; set; }
        public bool IsPersonal { get; set; }
        public ObservableCollection<SingleTopic> SingleTopics { get; set; }
        public string ButtonText { get; set; }

        public VotingResultLiveViewModel(VotingStartViewModel asvm) {
            CanSend = true;
            CanEdit = true;
            ButtonText = TextSend;
            TimeInSeconds = asvm.TimeInSeconds;
            _initialTime = asvm.TimeInSeconds;
            IsPersonal = asvm.IsPersonal;
            // TODO: REPLACE WITH REMOTE RESULTS
            ResultCollection = new ObservableCollection<Item>();
            if (IsPersonal) {
                SingleTopics = ((Group)asvm.ItemCollection[0]).topicList;
                CurrentSingleTopic = SingleTopics.FirstOrDefault();
            }
        }

        public async void weiterButton_Clicked(object sender, EventArgs e) {
            if (!_voteFinished) {
                CanSend = false;
                CanEdit = false;
                Device.StartTimer(TimeSpan.FromSeconds(1), Timer_OnElapsed);
                // TODO: Open websocket connection here to start
            } else {
                await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopModalAsync();
            }
        }

        private bool Timer_OnElapsed() {
            if (TimeInSeconds > 0) {
                TimeInSeconds -= 1;
                return true;
            }
            CanSend = true;
            CanEdit = true;
            if (IsPersonal) {
                CurrentSingleTopic.IsVotingDone = true;
                if (SingleTopics.Any(singleTopic => !singleTopic.IsVotingDone)) {
                    TimeInSeconds = _initialTime;
                    // Look if next SingleTopic in list still needs to vote and make it current, else look for first unvoted singletopic
                    var index = SingleTopics.IndexOf(CurrentSingleTopic);
                    if (index < SingleTopics.Count - 1 && !SingleTopics.ElementAt(index + 1).IsVotingDone) {
                        CurrentSingleTopic = SingleTopics.ElementAt(index + 1);
                    } else {
                        CurrentSingleTopic = SingleTopics.FirstOrDefault(x => x.IsVotingDone == false);
                    }
                }
            }
            CheckIfVoteFinished();
            return false;
        }

        private void CheckIfVoteFinished() {
            if (SingleTopics != null && SingleTopics.Any(singleTopic => !singleTopic.IsVotingDone)) {
                _voteFinished = false;
                if (CurrentSingleTopic.IsVotingDone) {
                    CanSend = false;
                    ButtonText = TextAlreadySent;
                } else {
                    CanSend = true;
                    ButtonText = TextSend;
                }
                return;
            }
            _voteFinished = true;
            ButtonText = TextExit;
        }

        public void timeEntry_OnFocused(object sender, FocusEventArgs e) {
            var entry = e.VisualElement as Entry;
            if (entry == null) {
                return;
            }
            // TODO: not mvvm style
            entry.Text = string.Empty;
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
                    await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopModalAsync();
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

        public void personPicker_ItemSelected(object sender, SelectedItemChangedEventArgs e) { CheckIfVoteFinished(); }
    }
}