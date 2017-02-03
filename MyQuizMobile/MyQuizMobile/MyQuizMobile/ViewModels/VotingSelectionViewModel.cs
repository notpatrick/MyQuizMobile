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
    public class VotingSelectionViewModel {
        private readonly List<Item> _items = new List<Item>();
        private readonly Networking _networking;
        public bool IsLoading { get; set; }
        public string SearchString { get; set; }
        public ObservableCollection<Item> ItemCollection { get; set; } = new ObservableCollection<Item>()
            ;
        public ItemType ItemType { get; set; }

        public VotingSelectionViewModel(Item item) {
            _networking = App.Networking;
            ItemType = item.ItemType;
        }

        public async Task GetAll() {
            IsLoading = true;
            await Task.Run(async () => {
                switch (ItemType) {
                case ItemType.Group:
                    var resultGroups = await _networking.Get<List<Group>>("api/groups/");
                    _items.Clear();
                    ItemCollection.Clear();
                    foreach (var g in resultGroups) {
                        _items.Add(g);
                        ItemCollection.Add(g);
                    }
                    break;
                case ItemType.QuestionBlock:
                    var resultQuestionBlock = await _networking.Get<List<QuestionBlock>>("api/questionBlock/");
                    _items.Clear();
                    ItemCollection.Clear();
                    foreach (var g in resultQuestionBlock) {
                        _items.Add(g);
                        ItemCollection.Add(g);
                    }
                    break;
                case ItemType.Question:
                    var resultQuestion = await _networking.Get<List<Question>>("api/questions/");
                    _items.Clear();
                    ItemCollection.Clear();
                    foreach (var g in resultQuestion) {
                        _items.Add(g);
                        ItemCollection.Add(g);
                    }
                    break;
                }
            });
            IsLoading = false;
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e) {
            Item res = null;
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
                IEnumerable<Item> filtered;
                if (SearchString == string.Empty) {
                    filtered = _items;
                    ItemCollection.Clear();
                    foreach (var g in filtered) {
                        ItemCollection.Add(g);
                    }
                    return;
                }
                filtered = _items.Where(x => x.DisplayText.ToLower().Contains(SearchString.ToLower()));
                ItemCollection.Clear();
                foreach (var g in filtered) {
                    ItemCollection.Add(g);
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
        public Item Item { get; set; }
    }
}