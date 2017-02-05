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
    public class VotingStartViewModel {
        private Item _selectedItem;
        private int _timeInSeconds;
        public ObservableCollection<Item> ItemCollection { get; set; }
        public Item SelectedItem {
            get { return _selectedItem; }
            set {
                _selectedItem = value;

                if (_selectedItem == null) {
                    return;
                }

                ItemTappedCommand.Execute(_selectedItem);
                SelectedItem = null;
            }
        }
        public int TimeInSeconds {
            get { return _timeInSeconds; }
            set {
                if (value < 1) {
                    _timeInSeconds = 1;
                } else if (TimeInSeconds > 999) {
                    _timeInSeconds = 999;
                } else {
                    _timeInSeconds = value;
                }
            }
        }
        public bool IsPersonal { get; set; }
        public bool CanSend { get; set; } = true;
        public bool GroupHasSingleTopics { get; set; }

        public ICommand ContinueButtonClickedCommand { get; private set; }
        public ICommand ItemTappedCommand { get; private set; }

        public VotingStartViewModel() { Init(); }

        private void Init() {
            SubscribeEvents();
            RegisterCommands();
            TimeInSeconds = 30;
            CanSend = false;
            ItemCollection = new ObservableCollection<Item> {
                new Group {Id = -1, ItemType = ItemType.Group, DisplayText = "Veranstaltung wählen"},
                new QuestionBlock {Id = -1, ItemType = ItemType.QuestionBlock, DisplayText = "Frageliste wählen"}
            };
        }

        private void SubscribeEvents() {
            MessagingCenter.Unsubscribe<VotingSelectionViewModel>(this, "PickDone");
            MessagingCenter.Subscribe<VotingSelectionViewModel, Item>(this, "PickDone",
                                                                      async (sender, arg) => {
                                                                          await SetItemAfterPick(arg);
                                                                      });
        }

        private void RegisterCommands() {
            ContinueButtonClickedCommand = new Command(async () => { await ContinueButtonClicked(); }, () => CanSend);
            ItemTappedCommand = new Command<Item>(async item => { await MenuItemTapped(item); });
        }

        private async Task SetItemAfterPick(Item item) {
            if (item != null) {
                switch (item.ItemType) {
                case ItemType.Group:
                    ItemCollection[0] = (Group)item;
                    if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows) {
                        ItemCollection[0] = (Group)item;
                    }
                    if (((Group)item).SingleTopics != null && ((Group)item).SingleTopics.Any()) {
                        GroupHasSingleTopics = true;
                        IsPersonal = true;
                    } else {
                        GroupHasSingleTopics = false;
                        IsPersonal = false;
                    }
                    break;
                case ItemType.QuestionBlock:
                    ItemCollection[1] = (QuestionBlock)item;
                    if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows) {
                        ItemCollection[1] = (QuestionBlock)item;
                    }
                    if (item.Id == 0) {
                        ItemCollection.Add(new Question {
                            Id = -1,
                            DisplayText = "Frage auswählen",
                            ItemType = ItemType.Question
                        });
                    } else if (ItemCollection.Any(x => x.ItemType == ItemType.Question)) {
                        ItemCollection.Remove(ItemCollection.First(x => x.ItemType == ItemType.Question));
                    }
                    break;
                case ItemType.Question:
                    ItemCollection[2] = (Question)item;
                    if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows) {
                        ItemCollection[2] = (Question)item;
                    }
                    break;
                }
                var veranstaltungPicked = ItemCollection[0].Id != -1;
                var fragenlistePicked = ItemCollection[1].Id != -1;
                var mustPickSingleQuestion = ItemCollection[1].Id == 0;
                if (veranstaltungPicked && fragenlistePicked) {
                    if (mustPickSingleQuestion) {
                        var singleQuestionPicked = ItemCollection[2].Id != -1;
                        CanSend = singleQuestionPicked;
                    } else {
                        CanSend = true;
                    }
                } else {
                    CanSend = false;
                }
            }
        }

        private async Task ContinueButtonClicked() {
            CanSend = false;
            ((Command)ContinueButtonClickedCommand).ChangeCanExecute();
            var nextPage = new VotingResultLivePage(this);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushModalAsync(
                                                                                                    new NavigationPage(
                                                                                                                       nextPage),
                                                                                                    true);
            CanSend = true;
            ((Command)ContinueButtonClickedCommand).ChangeCanExecute();
        }

        private async Task MenuItemTapped(Item item) {
            var nextPage = new VotingSelectionPage(item);
            MessagingCenter.Send(this, "Selected");
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushModalAsync(
                                                                                                    new NavigationPage(
                                                                                                                       nextPage),
                                                                                                    true);
        }
    }
}