﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyQuizMobile.DataModel {
    public partial class QuestionBlock : Item {
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
        public override string DisplayText { get { return Title; } set { Title = value; } }
        public override ItemType ItemType => ItemType.QuestionBlock;
        public override string DetailText => Questions != null ? Questions.Count > 0 ? $"{Questions.Count} Fragen" : "Enthält noch keine Fragen" : string.Empty;

        #region DELETE
        public static async Task DeleteById(long id) { await App.Networking.Delete($"api/questionBlock/{id}"); }
        #endregion

        #region POST
        public static async Task<QuestionBlock> Post(QuestionBlock questionBlock) { return await App.Networking.Post("api/questionBlock/", questionBlock); }

        public static async Task<QuestionBlock> Put(QuestionBlock questionBlock) { return await App.Networking.Put("api/questionBlock/", questionBlock); }
        #endregion

        #region GET
        public static async Task<List<QuestionBlock>> GetAll() { return await App.Networking.Get<List<QuestionBlock>>("api/questionBlock/"); }

        public static async Task<QuestionBlock> GetById(long id) { return await App.Networking.Get<QuestionBlock>($"api/questionBlock/{id}"); }
        #endregion
    }
}