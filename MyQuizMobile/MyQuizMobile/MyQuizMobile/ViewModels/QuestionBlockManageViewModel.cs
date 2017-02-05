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
    public class QuestionBlockManageViewModel {
        private List<QuestionBlock> _allQuestionBlocks = new List<QuestionBlock>();
        private bool _isSearching;
        private string _searchString = string.Empty;
        private Item _selectedItem;
        public bool IsLoading { get; set; }
        public ObservableCollection<QuestionBlock> QuestionBlocks { get; set; } =
            new ObservableCollection<QuestionBlock>();
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

        public ICommand AddCommand { get; private set; }
        public ICommand ItemSelectedCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }

        public QuestionBlockManageViewModel() { Init(); }

        private async void Init() {
            SubscribeEvents();
            RegisterCommands();
            await GetAllQuestionBlocks();
        }

        private void SubscribeEvents() {
            // TODO Questionblock edit view models
            MessagingCenter.Unsubscribe<QuestionBlockEditViewModel>(this, "Done");
            MessagingCenter.Subscribe<QuestionBlockEditViewModel, QuestionBlock>(this, "Done",
                                                                                 async (sender, arg) => {
                                                                                     await Finished();
                                                                                 });
            MessagingCenter.Unsubscribe<QuestionBlockEditViewModel>(this, "Canceled");
            MessagingCenter.Subscribe<QuestionBlockEditViewModel>(this, "Canceled", async sender => {
                await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopModalAsync(true);
                RefreshCommand.Execute(null);
            });
        }

        private void RegisterCommands() {
            AddCommand = new Command(Add);
            ItemSelectedCommand = new Command<Item>(async item => { await ItemSelected(item); });
            RefreshCommand = new Command(async () => { await GetAllQuestionBlocks(); });
            SearchCommand = new Command(Filter, () => !_isSearching && !IsLoading);
        }

        private async Task GetAllQuestionBlocks() {
            if (IsLoading) {
                return;
            }
            IsLoading = true;
            ((Command)RefreshCommand).ChangeCanExecute();
            await Task.Run(async () => { _allQuestionBlocks = await QuestionBlock.GetAll(); });
            IsLoading = false;
            ((Command)RefreshCommand).ChangeCanExecute();
            SearchCommand.Execute(null);
        }

        private async void Add() {
            var nextPage = new QuestionBlockEditPage(new QuestionBlock());
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushModalAsync(
                                                                                                    new NavigationPage(
                                                                                                                       nextPage),
                                                                                                    true);
        }

        private async Task Finished() {
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopModalAsync(true);
            RefreshCommand.Execute(null);
        }

        private void Filter() {
            _isSearching = true;
            ((Command)SearchCommand).ChangeCanExecute();
            var filtered = string.IsNullOrWhiteSpace(SearchString)
                               ? _allQuestionBlocks
                               : _allQuestionBlocks.Where(x => x.DisplayText.ToLower().Contains(SearchString.ToLower()));
            QuestionBlocks.Clear();
            foreach (var g in filtered) {
                QuestionBlocks.Add(g);
            }
            _isSearching = false;
            ((Command)SearchCommand).ChangeCanExecute();
        }

        private async Task ItemSelected(Item item) {
            var nextPage = new QuestionBlockEditPage((QuestionBlock)item);
            MessagingCenter.Send(this, "Selected");
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushModalAsync(
                                                                                                    new NavigationPage(
                                                                                                                       nextPage),
                                                                                                    true);
        }
    }
}