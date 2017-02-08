using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public partial class Question {
        public ObservableCollection<AnswerOption> AnswerOptions { get; set; } = new ObservableCollection<AnswerOption>();
        public override string DisplayText { get { return Text; } set { Text = value; } }
        public override string DetailText => AnswerOptions != null ? AnswerOptions.Count > 0 ? $"{AnswerOptions.Count} Antwortmöglichkeiten" : "Enthält noch keine Antwortmöglichkeiten" : string.Empty;
        public override ItemType ItemType => ItemType.Question;

        [JsonIgnore]
        public bool IsSelected { get; set; }
        [JsonIgnore]
        public int AnswerCount { get; set; } = 0;

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
}