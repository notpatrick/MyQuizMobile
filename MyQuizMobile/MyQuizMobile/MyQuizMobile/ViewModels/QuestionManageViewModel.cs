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
    public class QuestionManageViewModel {
        private List<Question> _allQuestions = new List<Question>();
        private bool _isSearching;
        private string _searchString = string.Empty;
        private Item _selectedItem;
        public bool IsLoading { get; private set; }
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
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

        public QuestionManageViewModel() { Init(); }

        private async void Init() {
            SubscribeEvents();
            RegisterCommands();
            await GetAllQuestions();
        }

        private void SubscribeEvents() {
            MessagingCenter.Unsubscribe<QuestionEditViewModel>(this, "Done");
            MessagingCenter.Subscribe<QuestionEditViewModel, Question>(this, "Done", async (sender, arg) => { await Finished(); });
        }

        private void RegisterCommands() {
            AddCommand = new Command(Add);
            ItemSelectedCommand = new Command<Item>(async item => { await ItemSelected(item); });
            RefreshCommand = new Command(async () => { await GetAllQuestions(); });
            SearchCommand = new Command(Filter, () => !_isSearching && !IsLoading);
        }

        private async Task GetAllQuestions() {
            if (IsLoading) {
                return;
            }
            IsLoading = true;
            ((Command)RefreshCommand).ChangeCanExecute();
            await Task.Run(async () => {
                _allQuestions = await Question.GetAll();
            });
            IsLoading = false;
            ((Command)RefreshCommand).ChangeCanExecute();
            SearchCommand.Execute(null);
        }

        private async void Add() {
            var nextPage = new QuestionEditPage(new Question());
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
        }

        private async Task Finished() {
            RefreshCommand.Execute(null);
        }

        private void Filter() {
            _isSearching = true;
            ((Command)SearchCommand).ChangeCanExecute();
            var filtered = string.IsNullOrWhiteSpace(SearchString) ? _allQuestions : _allQuestions.Where(x => x.DisplayText.ToLower().Contains(SearchString.ToLower()));
            Questions.Clear();
            foreach (var g in filtered) {
                Questions.Add(g);
            }
            _isSearching = false;
            ((Command)SearchCommand).ChangeCanExecute();
        }

        private async Task ItemSelected(Item item) {
            var nextPage = new QuestionEditPage((Question)item);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
        }
    }
}