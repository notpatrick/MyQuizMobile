using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class VotingSelectionViewModel {
        private readonly List<Item> _items = new List<Item>();
        private bool _isSearching;
        private string _searchString = string.Empty;
        private Item _selectedItem;
        public ObservableCollection<Item> ItemCollection { get; set; } = new ObservableCollection<Item>();
        public bool IsLoading { get; set; }
        public string SearchString {
            get { return _searchString; }
            set {
                _searchString = value;
                SearchCommand.Execute(null);
            }
        }
        public Item SelectedItem {
            get { return _selectedItem; }
            set {
                _selectedItem = value;
                if (_selectedItem == null) {
                    return;
                }
                ItemSelectedCommand.Execute(_selectedItem);
                SelectedItem = null;
            }
        }
        public ItemType ItemType { get; set; }

        public ICommand SearchCommand { get; private set; }
        public ICommand ItemSelectedCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public VotingSelectionViewModel(Item item) {
            ItemType = item.ItemType;
            Init();
        }

        private void Init() {
            RegisterCommands();
            RefreshCommand.Execute(null);
        }

        private void RegisterCommands() {
            SearchCommand = new Command(Filter, () => !_isSearching && !IsLoading);
            ItemSelectedCommand = new Command<Item>(ItemSelected);
            RefreshCommand = new Command(async () => { await GetAll(); }, () => !IsLoading);
        }

        private async Task GetAll() {
            if (IsLoading) {
                return;
            }
            IsLoading = true;
            ((Command)RefreshCommand).ChangeCanExecute();
            await Task.Run(async () => {
                switch (ItemType) {
                case ItemType.Group:
                    var resultGroups = await Group.GetAll();
                    _items.Clear();
                    foreach (var g in resultGroups) {
                        _items.Add(g);
                    }
                    break;
                case ItemType.QuestionBlock:
                    var resultQuestionBlock = await QuestionBlock.GetAll();
                    _items.Clear();
                    foreach (var g in resultQuestionBlock) {
                        _items.Add(g);
                    }
                    break;
                case ItemType.Question:
                    var resultQuestion = await Question.GetAll();
                    _items.Clear();

                    foreach (var g in resultQuestion) {
                        _items.Add(g);
                    }
                    break;
                }
            });
            IsLoading = false;
            ((Command)RefreshCommand).ChangeCanExecute();
            SearchCommand.Execute(null);
        }

        private void Filter() {
            _isSearching = true;
            ((Command)SearchCommand).ChangeCanExecute();
            var filtered = SearchString == string.Empty
                               ? _items
                               : _items.Where(x => x.DisplayText.ToLower().Contains(SearchString.ToLower()));
            ItemCollection.Clear();
            foreach (var g in filtered) {
                ItemCollection.Add(g);
            }
            _isSearching = false;
            ((Command)SearchCommand).ChangeCanExecute();
        }

        private void ItemSelected(Item item) {
            Item result = null;
            switch (ItemType) {
            case ItemType.Group:
                result = item as Group;
                break;
            case ItemType.QuestionBlock:
                result = item as QuestionBlock;
                break;
            case ItemType.Question:
                result = item as Question;
                break;
            }
            MessagingCenter.Send(this, "Selected");
            MessagingCenter.Send(this, "PickDone", result);
        }
    }
}