using System;
using System.Collections.ObjectModel;
using System.Linq;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class VotingStartViewModel {
        public ObservableCollection<Item> ItemCollection { get; set; }
        public int TimeInSeconds { get; set; }
        public bool IsPersonal { get; set; }
        public bool CanSend { get; set; }
        public bool GroupHasSingleTopics { get; set; }

        public VotingStartViewModel() {
            TimeInSeconds = 30;
            IsPersonal = false;
            CanSend = false;
            GroupHasSingleTopics = false;
            ItemCollection = new ObservableCollection<Item> {
                new Group {Id = -1, ItemType = ItemType.Group, DisplayText = "Veranstaltung wählen"},
                new QuestionBlock {Id = -1, ItemType = ItemType.QuestionBlock, DisplayText = "Frageliste wählen"}
            };
        }

        public async void OnMenuItemTapped(object sender, SelectedItemChangedEventArgs e) {
            var item = e.SelectedItem as Item;
            if (item == null) {
                return;
            }
            var nextPage = new VotingSelectionPage(item);
            nextPage.VotingSelectionViewModel.PickDone += SetItemAfterPick;
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
        }

        private async void SetItemAfterPick(object sender, MenuItemPickedEventArgs e) {
            var item = e.Item;
            if (item == null) {
                return;
            }
            switch (item.ItemType) {
            case ItemType.Group:
                ItemCollection[0] = (Group)item;
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows) {
                    ItemCollection[0] = (Group)item;
                }
                if (((Group)ItemCollection[0]).SingleTopics != null
                    && ((Group)ItemCollection[0]).SingleTopics.Any()) {
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

            var previousPage =
                await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
            ((VotingSelectionPage)previousPage).VotingSelectionViewModel.PickDone -= SetItemAfterPick;
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
                TimeInSeconds = 30;
            }
        }

        public void isPersonalSwitch_Toggled(object sender, ToggledEventArgs e) { IsPersonal = e.Value; }

        public async void continueButton_Clicked(object sender, EventArgs e) {
            var nextPage = new VotingResultLivePage(this);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
        }
    }
}