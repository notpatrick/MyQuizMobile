using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class QuestionBlockEditViewModel {
        public QuestionBlock QuestionBlock { get; set; }
        public bool CanDelete { get; set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand RemoveQuestionCommand { get; set; }
        public ICommand AddQuestionCommand { get; set; }

        public QuestionBlockEditViewModel(QuestionBlock qb) {
            RegisterCommads();
            SubscribeEvents();
            QuestionBlock = qb;
            CanDelete = QuestionBlock.Questions.Any();
        }

        private void RegisterCommads() {
            DeleteCommand = new Command(Delete);
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
            RemoveQuestionCommand = new Command<Question>(RemoveQuestion);
            AddQuestionCommand = new Command(Add);
        }

        private void SubscribeEvents()
        {
            MessagingCenter.Unsubscribe<QuestionBlockAddQuestionViewModel>(this, "PickDone");
            MessagingCenter.Subscribe<QuestionBlockAddQuestionViewModel, List<Question>>(this, "PickDone", async (s, args) => { await SetQuestions(args);
                await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopModalAsync(true);
            });

            MessagingCenter.Unsubscribe<QuestionBlockAddQuestionViewModel>(this, "Canceled");
            MessagingCenter.Subscribe<QuestionBlockAddQuestionViewModel>(this, "Canceled", async (m) => {
                await((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopModalAsync(true);
            });
        }

        private async Task SetQuestions(List<Question> list) {
            // TODO: Set questionlist with new input
        }

        private void SetQuestions(ObservableCollection<Question> list) { QuestionBlock.Questions = list; }

        private void Cancel() { MessagingCenter.Send(this, "Canceled"); }

        private async void Save() {
            QuestionBlock.Questions.Remove(x => string.IsNullOrWhiteSpace(x.Text));
            await QuestionBlock.Post(QuestionBlock);
            MessagingCenter.Send(this, "Done", QuestionBlock);
        }

        private async void Delete() {
            await QuestionBlock.DeleteById(QuestionBlock.Id);
            MessagingCenter.Send(this, "Done", QuestionBlock);
        }

        private async void Add() {
            var nextPage = new QuestionBlockAddQuestionPage(QuestionBlock);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushModalAsync(new NavigationPage(nextPage), true);
        }

        private void RemoveQuestion(Question q) {
            if (QuestionBlock.Questions.Contains(q)) {
                QuestionBlock.Questions.Remove(q);
            }
        }
    }
}