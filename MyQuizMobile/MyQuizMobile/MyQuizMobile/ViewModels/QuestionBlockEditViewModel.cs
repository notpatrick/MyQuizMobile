using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
            DeleteCommand = new Command(async () => { await Delete(); });
            SaveCommand = new Command(async () => { await Save(); });
            CancelCommand = new Command(async () => { await Cancel(); });
            RemoveQuestionCommand = new Command<Question>(RemoveQuestion);
            AddQuestionCommand = new Command(async () => { await Add(); });
        }

        private void SubscribeEvents() {
            MessagingCenter.Unsubscribe<QuestionBlockAddQuestionViewModel>(this, "Saved");
            MessagingCenter.Subscribe<QuestionBlockAddQuestionViewModel, ObservableCollection<Question>>(this, "Saved", async (s, args) => { await SetQuestions(args); });
        }

        private async Task SetQuestions(ObservableCollection<Question> list) { QuestionBlock.Questions = list; }

        private async Task Cancel() { await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true); }

        private async Task Save() {
            QuestionBlock.Questions.Remove(x => string.IsNullOrWhiteSpace(x.Text));
            await QuestionBlock.Post(QuestionBlock);
            MessagingCenter.Send(this, "Done", QuestionBlock);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
        }

        private async Task Delete() {
            await QuestionBlock.DeleteById(QuestionBlock.Id);
            MessagingCenter.Send(this, "Done", QuestionBlock);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
        }

        private async Task Add() {
            var nextPage = new QuestionBlockAddQuestionPage(QuestionBlock);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
        }

        private void RemoveQuestion(Question q) {
            if (QuestionBlock.Questions.Contains(q)) {
                QuestionBlock.Questions.Remove(q);
            }
        }
    }
}