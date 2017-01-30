using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyQuizMobile;

namespace MyQuizMobile
{
    public class AbstimmungStartenViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IMenuItem> OptionCollection { get; set; }
        private int _timeInSeconds;
        public int TimeInSeconds
        {
            get { return _timeInSeconds; }
            set
            {
                _timeInSeconds = value;
                OnPropertyChanged("TimeInSeconds");
            }
        }
        private bool _isPersonenbezogen;
        public bool IsPersonenbezogen
        {
            get { return _isPersonenbezogen; }
            set
            {
                _isPersonenbezogen = value;
                OnPropertyChanged("IsPersonenbezogen");
            }
        }
        private bool _canSend;
        public bool CanSend
        {
            get { return _canSend; }
            set
            {
                _canSend = value;
                OnPropertyChanged("CanSend");
            }
        }


        public AbstimmungStartenViewModel()
        {
            TimeInSeconds = 30;
            IsPersonenbezogen = false;
            CanSend = false;
            OptionCollection = new ObservableCollection<IMenuItem>
            {
                new MenuItem() {Id = -1, ItemType = ItemType.Veranstaltung},
                new MenuItem() {Id = -1, ItemType = ItemType.Frageliste}
            };
        }

        public async void OnMenuItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as IMenuItem;

            if (item == null) return;
            var nextPage = new AuswahlPage(item);
            nextPage.AuswahlViewModel.PickDone += SetItemAfterPick;
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
            (sender as ListView).SelectedItem = null;
        }

        private async void SetItemAfterPick(object sender, MenuItemPickedEventArgs e)
        {
            var item = e.Item;
            if (item == null) return;
            switch (item.ItemType)
            {
                case ItemType.Veranstaltung:
                    OptionCollection[0] = item;
                    break;
                case ItemType.Frageliste:
                    OptionCollection[1] = item;
                    if (item.Id == 0)
                    {
                        OptionCollection.Add(new MenuItem()
                        {
                            Id = -1,
                            DisplayText = "Frage auswählen",
                            ItemType = ItemType.Frage
                        });
                    }
                    else if (OptionCollection.Count > 2)
                    {
                        OptionCollection.RemoveAt(2);
                    }
                    break;
                case ItemType.Frage:
                    OptionCollection[2] = item;
                    break;
            }
            var veranstaltungPicked = OptionCollection[0].Id != -1;
            var fragenlistePicked = OptionCollection[1].Id != -1;
            var mustPickSingleQuestion = OptionCollection[1].Id == 0;
            if (veranstaltungPicked && fragenlistePicked)
            {
                if (mustPickSingleQuestion)
                {
                    var singleQuestionPicked = OptionCollection[2].Id != -1;
                    CanSend = singleQuestionPicked;
                }
                else
                {
                    CanSend = true;
                }
            }
            else
            {
                CanSend = false;
            }
            var previousPage = await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
            (previousPage as AuswahlPage).AuswahlViewModel.PickDone -= SetItemAfterPick;
        }

        public void timeEntry_OnFocused(object sender, FocusEventArgs e)
        {
            var entry = e.VisualElement as Entry;
            if (entry == null) return;
            // TODO: not mvvm style
            entry.Text = "";
        }

        public void timeEntry_OnUnfocused(object sender, FocusEventArgs e)
        {
            var entry = e.VisualElement as Entry;
            if (entry == null) return;
            int newTime;
            if (int.TryParse(entry.Text, out newTime))
            {
                if (newTime < 1)
                    newTime = 1;
                else if (newTime > 9999)
                    newTime = 9999;
                TimeInSeconds = newTime;
            }
            else
            {
                TimeInSeconds = 30;
            }
        }

        public void weiterButton_Clicked(object sender, EventArgs e)
        {
            
        }
        #region inotify
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public void personenbezogenSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            IsPersonenbezogen = e.Value;
        }
    }

    public class MenuItem : IMenuItem
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public ItemType ItemType { get; set; }
    }
}
