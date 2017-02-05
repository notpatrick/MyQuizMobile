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
            QuestionBlock = qb;
            CanDelete = QuestionBlock.Questions.Any();
            DeleteCommand = new Command(Delete);
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
            RemoveQuestionCommand = new Command<Question>(RemoveQuestion);
            AddQuestionCommand = new Command(Add);
        }

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

        private void Add() { QuestionBlock.Questions.Add(new Question {Text = "Neue Frage"}); }

        private void RemoveQuestion(Question q) {
            if (QuestionBlock.Questions.Contains(q)) {
                QuestionBlock.Questions.Remove(q);
            }
        }
    }
}