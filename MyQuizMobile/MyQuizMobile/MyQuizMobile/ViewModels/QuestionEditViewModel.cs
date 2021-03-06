﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class QuestionEditViewModel {
        public ObservableCollection<string> Types { get; set; } = new ObservableCollection<string> {Constants.QuestionTypeSingleText, Constants.QuestionTypeMultiText};
        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string> {Constants.QuestionCategoryVoteText, Constants.QuestionCategoryQuizText};
        public Question Question { get; set; }
        public bool CanDelete { get; set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand RemoveAnswerOptionCommand { get; set; }
        public ICommand AddAnswerCommand { get; set; }

        public QuestionEditViewModel(Question q) {
            Question = q;
            CanDelete = Question.AnswerOptions.Any();
            DeleteCommand = new Command(async () => { await Delete(); });
            SaveCommand = new Command(async () => { await Save(); });
            CancelCommand = new Command(async () => { await Cancel(); });
            RemoveAnswerOptionCommand = new Command<AnswerOption>(RemoveAnswer);
            AddAnswerCommand = new Command(Add);
        }

        private async Task Cancel() { await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true); }

        private async Task Save() {
            Question.AnswerOptions.Remove(x => string.IsNullOrWhiteSpace(x.Text));
            await Question.Post(Question);
            MessagingCenter.Send(this, "Done", Question);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
        }

        private async Task Delete() {
            await Question.DeleteById(Question.Id);
            MessagingCenter.Send(this, "Done", Question);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
        }

        private void Add() { Question.AnswerOptions.Add(new AnswerOption()); }

        private void RemoveAnswer(AnswerOption q) {
            if (Question.AnswerOptions.Contains(q)) {
                Question.AnswerOptions.Remove(q);
            }
        }
    }
}