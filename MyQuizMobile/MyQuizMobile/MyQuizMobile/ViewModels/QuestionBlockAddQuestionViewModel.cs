using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile
{
    [NotifyPropertyChanged]
    public class QuestionBlockAddQuestionViewModel
    {
        private readonly List<Question> _items = new List<Question>();
        private bool _isSearching;
        private string _searchString = string.Empty;
        public ObservableCollection<Question> ItemCollection { get; set; } = new ObservableCollection<Question>();
        public bool IsLoading { get; set; }
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                SearchCommand.Execute(null);
            }
        }
        public ICommand SearchCommand { get; private set; }
        public ICommand ItemSelectedCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public QuestionBlockAddQuestionViewModel()
        {
            Init();
        }

        private void Init()
        {
            RegisterCommands();
            RefreshCommand.Execute(null);
        }

        private void RegisterCommands()
        {
            SearchCommand = new Command(Filter, () => !_isSearching && !IsLoading);
            ItemSelectedCommand = new Command<Question>(async i => { await ItemSelected(i); });
            RefreshCommand = new Command(async () => { await GetAll(); }, () => !IsLoading);
        }

        private async Task GetAll()
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;
            ((Command)RefreshCommand).ChangeCanExecute();
            await Task.Run(async () => {
                var resultQuestion = await Question.GetAll();
                _items.Clear();

                foreach (var g in resultQuestion)
                {
                    _items.Add(g);
                }
            });
            IsLoading = false;
            ((Command)RefreshCommand).ChangeCanExecute();
            SearchCommand.Execute(null);
        }

        private void Filter()
        {
            _isSearching = true;
            ((Command)SearchCommand).ChangeCanExecute();
            var filtered = SearchString == string.Empty ? _items : _items.Where(x => x.DisplayText.ToLower().Contains(SearchString.ToLower()));
            ItemCollection.Clear();
            foreach (var g in filtered)
            {
                ItemCollection.Add(g);
            }
            _isSearching = false;
            ((Command)SearchCommand).ChangeCanExecute();
        }

        private async Task ItemSelected(Question item)
        {
            // TODO add item to selecteditems list
        }

        private async Task Save() {

            // TODO send selected items via mesaage
            MessagingCenter.Send(this, "PickDone");
        }
    }
}
