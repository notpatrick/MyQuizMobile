﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PostSharp.Patterns.Model;

namespace MyQuizMobile.DataModel {
    [NotifyPropertyChanged]
    public partial class Question {
        public ObservableCollection<AnswerOption> Answers { get; set; } = new ObservableCollection<AnswerOption>();
        public override string DisplayText { get { return Text; } set { Text = value; } }
        public override string DetailText => $"{Answers.Count} Antwortmöglichkeiten";
        public override ItemType ItemType => ItemType.Question;
        [JsonIgnore]
        public bool HasCorrectAnswers => Category == "Quiz";
        [JsonIgnore]
        public string TypText => MultipleChoice == "multi" ? "Multiple Choice" : "Single Choice";

        #region POST
        public static async Task<Question> Post(Question question) { return await App.Networking.Post("api/questions/", question); }
        #endregion

        #region DELETE
        public static async Task DeleteById(long id) { await App.Networking.Delete($"api/questions/{id}"); }
        #endregion

        #region GET
        public static async Task<List<Question>> GetAll() { return await App.Networking.Get<List<Question>>("api/questions/"); }

        public static async Task<Question> GetById(long id) { return await App.Networking.Get<Question>($"api/questions/{id}"); }
        #endregion
    }

    public enum QuestionCategory {
        Vote,
        Quiz
    }
}