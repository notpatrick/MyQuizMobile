using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class VotingSelectionViewModel {
        private readonly List<Item> _items = new List<Item>();
        public bool IsLoading { get; set; }
        public string SearchString { get; set; }
        public ObservableCollection<Item> ItemCollection { get; set; } = new ObservableCollection<Item>()
            ;
        public ItemType ItemType { get; set; }

        public VotingSelectionViewModel(Item item) { ItemType = item.ItemType; }

        public async Task GetAll() {
            IsLoading = true;
            await Task.Run(async () => {
                switch (ItemType) {
                case ItemType.Group:
                    var resultGroups = await Group.GetAll();
                    _items.Clear();
                    ItemCollection.Clear();
                    foreach (var g in resultGroups) {
                        _items.Add(g);
                        ItemCollection.Add(g);
                    }
                    break;
                case ItemType.QuestionBlock:
                    var resultQuestionBlock = await QuestionBlock.GetAll();
                    _items.Clear();
                    ItemCollection.Clear();
                    foreach (var g in resultQuestionBlock) {
                        _items.Add(g);
                        ItemCollection.Add(g);
                    }
                    break;
                case ItemType.Question:
                    var resultQuestion = await Question.GetAll();
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

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e) {
            Item result = null;
            switch (ItemType) {
            case ItemType.Group:
                result = e.SelectedItem as Group;
                break;
            case ItemType.QuestionBlock:
                result = e.SelectedItem as QuestionBlock;
                break;
            case ItemType.Question:
                result = e.SelectedItem as Question;
                break;
            }
            if (result == null) {
                return;
            }
            OnPicked(new MenuItemPickedEventArgs {Item = result});
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