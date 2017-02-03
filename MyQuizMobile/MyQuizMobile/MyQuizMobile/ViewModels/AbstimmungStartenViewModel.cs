using System;
using System.Collections.ObjectModel;
using System.Linq;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class AbstimmungStartenViewModel {
        public ObservableCollection<MenuItem> OptionCollection { get; set; }
        public int TimeInSeconds { get; set; }
        public bool IsPersonenbezogen { get; set; }
        public bool CanSend { get; set; }
        public bool VeranstaltungHasPersons { get; set; }

        public AbstimmungStartenViewModel() {
            TimeInSeconds = 30;
            IsPersonenbezogen = false;
            CanSend = false;
            VeranstaltungHasPersons = false;
            OptionCollection = new ObservableCollection<MenuItem> {
                new Group {Id = -1, ItemType = ItemType.Group, DisplayText = "Veranstaltung wählen"},
                new QuestionBlock {Id = -1, ItemType = ItemType.QuestionBlock, DisplayText = "Frageliste wählen"}
            };
        }

        public async void OnMenuItemTapped(object sender, SelectedItemChangedEventArgs e) {
            var item = e.SelectedItem as MenuItem;
            if (item == null) {
                return;
            }
            var nextPage = new AuswahlPage(item);
            nextPage.AuswahlViewModel.PickDone += SetItemAfterPick;
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
        }

        private async void SetItemAfterPick(object sender, MenuItemPickedEventArgs e) {
            var item = e.Item;
            if (item == null) {
                return;
            }
            switch (item.ItemType) {
            case ItemType.Group:
                OptionCollection[0] = (Group)item;
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows) {
                    OptionCollection[0] = (Group)item;
                }
                if (((Group)OptionCollection[0]).SingleTopics != null && ((Group)OptionCollection[0]).SingleTopics.Any()) {
                    VeranstaltungHasPersons = true;
                    IsPersonenbezogen = true;
                } else {
                    VeranstaltungHasPersons = false;
                    IsPersonenbezogen = false;
                }
                break;
            case ItemType.QuestionBlock:
                OptionCollection[1] = (QuestionBlock)item;
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows) {
                    OptionCollection[1] = (QuestionBlock)item;
                }
                if (item.Id == 0) {
                    OptionCollection.Add(new Question {
                        Id = -1,
                        DisplayText = "Frage auswählen",
                        ItemType = ItemType.Question
                    });
                } else if (OptionCollection.Any(x => x.ItemType == ItemType.Question)) {
                    OptionCollection.Remove(OptionCollection.First(x => x.ItemType == ItemType.Question));
                }
                break;
            case ItemType.Question:
                OptionCollection[2] = (Question)item;
                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows) {
                    OptionCollection[2] = (Question)item;
                }
                break;
            }
            var veranstaltungPicked = OptionCollection[0].Id != -1;
            var fragenlistePicked = OptionCollection[1].Id != -1;
            var mustPickSingleQuestion = OptionCollection[1].Id == 0;
            if (veranstaltungPicked && fragenlistePicked) {
                if (mustPickSingleQuestion) {
                    var singleQuestionPicked = OptionCollection[2].Id != -1;
                    CanSend = singleQuestionPicked;
                } else {
                    CanSend = true;
                }
            } else {
                CanSend = false;
            }
            var previousPage = await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
            ((AuswahlPage)previousPage).AuswahlViewModel.PickDone -= SetItemAfterPick;
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

        public void personenbezogenSwitch_Toggled(object sender, ToggledEventArgs e) { IsPersonenbezogen = e.Value; }

        public async void weiterButton_Clicked(object sender, EventArgs e) {
            var nextPage = new LiveResultPage(this);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
        }
    }
}