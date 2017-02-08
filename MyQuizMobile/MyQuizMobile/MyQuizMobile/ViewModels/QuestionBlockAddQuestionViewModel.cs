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
    public class QuestionBlockAddQuestionViewModel {
        private readonly List<Question> _items = new List<Question>();
        private bool _isSearching;
        private string _searchString = string.Empty;
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
        public ObservableCollection<Question> SelectedQuestions { get; set; } = new ObservableCollection<Question>();
        public bool IsLoading { get; set; }
        public string SearchString {
            get { return _searchString; }
            set {
                _searchString = value;
                SearchCommand.Execute(null);
            }
        }
        public ICommand SearchCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand AcceptCommand { get; private set; }

        public QuestionBlockAddQuestionViewModel(QuestionBlock qb) { Init(qb); }

        private void Init(QuestionBlock qb) {
            RegisterCommands();
            foreach (var selectedQuestion in qb.Questions) {
                selectedQuestion.IsSelected = true;
                SelectedQuestions.Add(selectedQuestion);
            }
            RefreshCommand.Execute(null);
        }

        private void RegisterCommands() {
            SearchCommand = new Command(Filter, () => !_isSearching && !IsLoading);
            RefreshCommand = new Command(async () => { await GetAll(); }, () => !IsLoading);
            CancelCommand = new Command(async () => { await Cancel(); });
            AcceptCommand = new Command(async () => { await Save(); });
        }

        private async Task GetAll() {
            if (IsLoading) {
                return;
            }
            IsLoading = true;
            ((Command)RefreshCommand).ChangeCanExecute();
            await Task.Run(async () => {
                var resultQuestion = await Question.GetAll();
                _items.Clear();

                foreach (var g in resultQuestion) {
                    var x = SelectedQuestions.FirstOrDefault(q => q.Id == g.Id);
                    if (x != null)
                        g.IsSelected = x.IsSelected;
                    _items.Add(g);
                }
            });
            IsLoading = false;
            ((Command)RefreshCommand).ChangeCanExecute();
            SearchCommand.Execute(null);
        }

        private void Filter() {
            _isSearching = true;
            ((Command)SearchCommand).ChangeCanExecute();
            var filtered = SearchString == string.Empty ? _items : _items.Where(x => x.DisplayText.ToLower().Contains(SearchString.ToLower()));
            Questions.Clear();
            foreach (var g in filtered) {
                Questions.Add(g);
            }
            _isSearching = false;
            ((Command)SearchCommand).ChangeCanExecute();
        }

        public void Switched(object sender, ToggledEventArgs e) {
            var q = ((SwitchCell)sender).BindingContext as Question;
            if (SelectedQuestions.Any(x => x.Id == q?.Id))
                SelectedQuestions.First(x => x.Id == q?.Id).IsSelected = e.Value;
            else {
                SelectedQuestions.Add(q);
            }
        }

        private async Task Save() {
            MessagingCenter.Send(this, "Saved", SelectedQuestions);
            await((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
        }

        private async Task Cancel()
        {
            await((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
        }

    }
}