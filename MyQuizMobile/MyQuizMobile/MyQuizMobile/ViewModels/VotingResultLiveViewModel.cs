using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private SingleTopic _currentSingleTopic;
        private int _initialTime;
        private int _timeInSeconds;
        private bool _voteFinished;
        private Socket _socket = App.Socket;

        public ObservableCollection<Item> ResultCollection { get; set; } = new ObservableCollection<Item>();
        public int TimeInSeconds {
            get { return _timeInSeconds; }
            set {
                if (value < 0) {
                    _timeInSeconds = 0;
                } else if (TimeInSeconds > 999) {
                    _timeInSeconds = 999;
                } else {
                    _timeInSeconds = value;
                }
            }
        }
        public bool CanSend { get; set; }
        public bool CanEdit { get; set; }
        public bool IsPersonal { get; set; }
        public Group Group { get; set; }
        public ObservableCollection<SingleTopic> SingleTopics { get; set; }
        public string ButtonText { get; set; }
        public SingleTopic CurrentSingleTopic {
            get { return _currentSingleTopic; }
            set {
                _currentSingleTopic = value;

                if (_currentSingleTopic == null) {
                    return;
                }

                SingleTopicSelectedCommand.Execute(_currentSingleTopic);
            }
        }

        public ICommand ButtonClickedCommand { get; private set; }
        public ICommand SingleTopicSelectedCommand { get; private set; }

        public VotingResultLiveViewModel(VotingStartViewModel asvm) { Init(asvm); }

        private void Init(VotingStartViewModel asvm) {
            RegisterCommands();
            CanSend = true;
            CanEdit = true;
            ButtonText = TextSend;
            TimeInSeconds = asvm.TimeInSeconds;
            _initialTime = asvm.TimeInSeconds;
            IsPersonal = asvm.IsPersonal;
            if (IsPersonal) {
                SingleTopics = ((Group)asvm.ItemCollection[0]).SingleTopics;
                CurrentSingleTopic = SingleTopics.FirstOrDefault();
            }
        }

        private void RegisterCommands() {
            ButtonClickedCommand = new Command(async () => { await Start(); });
            SingleTopicSelectedCommand = new Command<SingleTopic>(CheckSingleTopic);
        }

        public async Task Start() {
            if (!_voteFinished) {
                CanSend = false;
                CanEdit = false;
                Device.StartTimer(TimeSpan.FromSeconds(1), TimerElapsed);
                // TODO: Open websocket connection here to start

                
            } else {
                await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopModalAsync();
            }
        }

        private bool TimerElapsed() {
            if (TimeInSeconds > 0) {
                TimeInSeconds -= 1;
                return true;
            }
            CanSend = true;
            CanEdit = true;
            TimeInSeconds = _initialTime;
            if (!_voteFinished && IsPersonal) {
                CurrentSingleTopic.IsVotingDone = true;
                if (SingleTopics.Any(singleTopic => !singleTopic.IsVotingDone)) {
                    // Look if next SingleTopic in list still needs to vote and make it current, else look for first unvoted singletopic
                    var index = SingleTopics.IndexOf(CurrentSingleTopic);
                    if (index < SingleTopics.Count - 1 && !SingleTopics.ElementAt(index + 1).IsVotingDone) {
                        CurrentSingleTopic = SingleTopics.ElementAt(index + 1);
                    } else {
                        CurrentSingleTopic = SingleTopics.FirstOrDefault(x => x.IsVotingDone == false);
                    }
                }
            }
            IsVoteFinished();
            return false;
        }

        private void CheckSingleTopic(SingleTopic st) {
            if (st.IsVotingDone) {
                CanSend = false;
                ButtonText = TextAlreadySent;
            } else {
                CanSend = true;
                ButtonText = TextSend;
            }
        }

        private bool IsVoteFinished() {
            if (SingleTopics != null && SingleTopics.Any(singleTopic => !singleTopic.IsVotingDone)) {
                _voteFinished = false;
                return _voteFinished;
            }
            _voteFinished = true;
            ButtonText = TextExit;
            return _voteFinished;
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
    }
}