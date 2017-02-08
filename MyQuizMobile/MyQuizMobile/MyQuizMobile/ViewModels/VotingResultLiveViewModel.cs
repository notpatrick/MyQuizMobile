using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyQuizMobile.DataModel;
using MyQuizMobile.Helpers;
using Newtonsoft.Json;
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
        private long _initialTime;
        private long _timeInSeconds;
        private bool _voteFinished;
        private DateTime DestinationTime;
        private Socket prevSocket;
        private VotingStartViewModel vsvm;

        public ObservableCollection<Question> ResultCollection { get; set; }
        public long TimeInSeconds {
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
        public QuestionBlock QuestionBlock { get; set; }
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

        public int SurveyId { get; set; }

        public ICommand ButtonClickedCommand { get; private set; }
        public ICommand SingleTopicSelectedCommand { get; private set; }

        public VotingResultLiveViewModel(VotingStartViewModel asvm) { Init(asvm); }

        private void Init(VotingStartViewModel vm) {
            RegisterCommands();
            vsvm = vm;
            ResultCollection = new ObservableCollection<Question>();
            Group = vm.ItemCollection[0] as Group;
            QuestionBlock = vm.ItemCollection[1] as QuestionBlock;
            CanSend = true;
            CanEdit = true;
            ButtonText = TextSend;
            TimeInSeconds = vm.TimeInSeconds;
            _initialTime = vm.TimeInSeconds;
            IsPersonal = vm.IsPersonal;
            if (IsPersonal) {
                SingleTopics = ((Group)vm.ItemCollection[0]).SingleTopics;
                CurrentSingleTopic = SingleTopics.FirstOrDefault();
            } else {
                CurrentSingleTopic = null;
            }
            // TODO: Create GivenAnswer from VotingStartViewModel
        }

        private void RegisterCommands() {
            ButtonClickedCommand = new Command(async () => { await Start(); });
            SingleTopicSelectedCommand = new Command<SingleTopic>(CheckSingleTopic);
        }

        public async Task Start() {
            if (!_voteFinished) {
                CanSend = false;
                CanEdit = false;
                Device.StartTimer(TimeSpan.FromMilliseconds(200), TimerElapsed);

                // Prepare POST message to initiate vote
                var s = CurrentSingleTopic;
                var now = TimeHelper.ConvertToUnixTimestamp(DateTime.Now);
                var destUnix = (int)(now + TimeInSeconds);
                DestinationTime = DateTime.Now + TimeSpan.FromSeconds(TimeInSeconds);

                var givenAnswersToSend = QuestionBlock?.Questions.Select(q => new GivenAnswer {Group = Group, QuestionBlock = QuestionBlock, Question = q, SingleTopic = s, TimeStamp = destUnix.ToString()}).ToList();

                ResultCollection.Clear();
                foreach (var quest in QuestionBlock?.Questions) {
                    quest.AnswerCount = 0;
                    ResultCollection.Add(quest);
                }

                // Send POST
                try {
                    var result = await GivenAnswer.Start(givenAnswersToSend, TimeInSeconds);
                    SurveyId = (int)result.First().SurveyId;
                } catch (Exception) {
                    throw;
                }

                // Create Socket connection with surveyId
                try {
                    var close = prevSocket?.Close();
                    if (close != null) {
                        await close;
                    }
                    var sock = new Socket();
                    prevSocket = sock;
                    await sock.Connect(SurveyId);
                    sock.ReceiveLoop(incomingString => {
                        try {
                            var givenanswer = JsonConvert.DeserializeObject<GivenAnswer>(incomingString);
                            if (givenanswer == null) {
                                return;
                            }
                            var q = ResultCollection.First(x => x.Id == givenanswer.Question.Id);
                            q.AnswerCount++;
                        } catch (JsonSerializationException e) {
                            e.Data.Add("incoming", incomingString);
                            throw e;
                        }
                    });
                } catch (Exception) {
                    throw;
                }
            } else {
                await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopModalAsync(true);
                ((MasterDetailPage)Application.Current.MainPage).Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(VotingStartPage)));
            }
        }

        private bool TimerElapsed() {
            var remaining = DestinationTime.Subtract(DateTime.Now).Seconds;
            if (remaining > 0) {
                TimeInSeconds = remaining;
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
                var result = await page.DisplayAlert("Achtung!", "Wollen Sie die aktuelle Umfrage wirklich vorzeitig beenden? Ergebnisse können dann unvollständig sein!", "Umfrage Beenden", "Zurück");
                if (result) {
                    await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopModalAsync(true);
                    ((MasterDetailPage)Application.Current.MainPage).Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(VotingStartPage)));
                }
            });

            return true;
        }
    }
}