using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public class Question : Item {
        public override int Id { get; set; }
        public string Text { get; set; }
        public QuestionCategory Category { get; set; }
        public bool MultipleChoice { get; set; }
        [JsonIgnore]
        public virtual List<AnswerOption> AnswerOptions { get; set; }
        public override string DisplayText { get { return Text; } set { Text = value; } }
        public override ItemType ItemType => ItemType.Question;

        #region POST
        public static async Task<Question> Post(Question question) {
            return await App.Networking.Post("api/questions/", question);
        }
        #endregion

        #region DELETE
        public static async void DeleteById(int id) { await App.Networking.Delete($"api/questions/{id}"); }
        #endregion

        #region GET
        public static async Task<List<Question>> GetAll() {
            return await App.Networking.Get<List<Question>>("api/questions/");
        }

        public static async Task<Question> GetById(int id) {
            return await App.Networking.Get<Question>($"api/questions/{id}");
        }
        #endregion
    }

    public enum QuestionCategory {
        Vote,
        Quiz
    }
}