using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public class Question : MenuItem {
        public override int Id { get; set; }
        public string Text { get; set; }
        public QuestionCategory Category { get; set; }
        public bool MultipleChoice { get; set; }
        [JsonIgnore]
        public virtual List<AnswerOption> AnswerOptions { get; set; }
        public override string DisplayText { get { return Text; } set { Text = value; } }
        public override ItemType ItemType => ItemType.Question;

        #region POST
        public static async Task<Question> Post(Question g) {
            var question = await App.Networking.Post("api/questions/", g);
            return question;
        }
        #endregion

        #region DELETE
        public static async void DeleteById(int i) { await App.Networking.Delete($"api/questions/{i}"); }
        #endregion

        #region GET
        public static async Task<List<Question>> GetAll() {
            return await App.Networking.Get<List<Question>>("api/questions/");
        }

        public static async Task<Question> GetById(int i) {
            return await App.Networking.Get<Question>($"api/questions/{i}");
        }
        #endregion
    }

    public enum QuestionCategory {
        Umfrage,
        Quiz
    }
}