using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MyQuizMobile.DataModel;
using MYQuizMobile;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class AuswahlViewModel {
        private readonly List<MenuItem> _allItems = new List<MenuItem>();
        private readonly Networking _networking;
        public bool IsLoading { get; set; }
        public string SearchString { get; set; }
        public ObservableCollection<MenuItem> AuswahlItemCollection { get; set; } = new ObservableCollection<MenuItem>()
            ;
        public ItemType ItemType { get; set; }

        public AuswahlViewModel(MenuItem item) {
            _networking = App.Networking;
            ItemType = item.ItemType;
        }

        public async Task GetAll() {
            IsLoading = true;
            await Task.Run(async () => {
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
            });
            IsLoading = false;
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e) {
            MenuItem res = null;
            switch (ItemType) {
            case ItemType.Group:
                res = e.SelectedItem as Group;
                break;
            case ItemType.QuestionBlock:
                res = e.SelectedItem as QuestionBlock;
                break;
            case ItemType.Question:
                res = e.SelectedItem as Question;
                break;
            }
            if (res == null) {
                return;
            }
            OnPicked(new MenuItemPickedEventArgs {Item = res});
        }

        public async Task Filter() {
            await Task.Run(() => {
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
            });
        }

        public async void listView_Refreshing(object sender, EventArgs e) { await GetAll(); }
        public async void searchBar_TextChanged(object sender, TextChangedEventArgs e) { await Filter(); }
        public async void OnAppearing(object sender, EventArgs e) { await GetAll(); }

        #region event
        public event MenuItemPickedHanler PickDone;

        public delegate void MenuItemPickedHanler(object sender, MenuItemPickedEventArgs e);

        protected virtual void OnPicked(MenuItemPickedEventArgs e) { PickDone?.Invoke(this, e); }
        #endregion
    }

    public class MenuItemPickedEventArgs : EventArgs {
        public MenuItem Item { get; set; }
    }
}