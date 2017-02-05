using System.Linq;
using System.Windows.Input;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class QuestionEditViewModel {
        public Question Question { get; set; }
        public bool CanDelete { get; set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand RemoveAnswerOptionCommand { get; set; }
        public ICommand AddSingleTopicCommand { get; set; }

        public QuestionEditViewModel(Question q) {
            Question = q;
            CanDelete = Question.Answers.Any();
            DeleteCommand = new Command(Delete);
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
            RemoveAnswerOptionCommand = new Command<AnswerOption>(RemoveAnswer);
            AddSingleTopicCommand = new Command(Add);
        }

        private void Cancel() { MessagingCenter.Send(this, "Canceled"); }

        private async void Save() {
            Question.Answers.Remove(x => string.IsNullOrWhiteSpace(x.Text));
            await Question.Post(Question);
            MessagingCenter.Send(this, "Done", Question);
        }

        private async void Delete() {
            await Question.DeleteById(Question.Id);
            MessagingCenter.Send(this, "Done", Question);
        }

        private void Add() { Question.Answers.Add(new AnswerOption()); }

        private void RemoveAnswer(AnswerOption q) {
            if (Question.Answers.Contains(q)) {
                Question.Answers.Remove(q);
            }
        }
    }
}