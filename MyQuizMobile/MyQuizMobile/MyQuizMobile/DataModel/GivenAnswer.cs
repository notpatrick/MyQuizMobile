using System.Collections.Generic;
using System.Threading.Tasks;
using PostSharp.Patterns.Model;

namespace MyQuizMobile.DataModel {
    public class GivenAnswer : Item {
        public long SurveyId { get; set; }
        public string TimeStamp { get; set; }
        public QuestionBlock QuestionBlock { get; set; }
        public Question Question { get; set; }
        public AnswerOption AnswerOption { get; set; }
        public SingleTopic SingleTopic { get; set; }
        public Group Group { get; set; }
        public Device Device { get; set; }
        public override long Id { get; set; }
        public override string DisplayText => $"Given Answer {Id}";
        public override ItemType ItemType => ItemType.GivenAnswer;

        #region POST
        public static async Task<GivenAnswer> Post(GivenAnswer givenAnswer) { return await App.Networking.Post("api/groups/", givenAnswer); }
        #endregion

        #region DELETE
        public static async void DeleteById(long id) { await App.Networking.Delete($"api/groups/{id}"); }
        #endregion

        #region GET
        public static async Task<List<GivenAnswer>> GetAll() { return await App.Networking.Get<List<GivenAnswer>>("api/groups/"); }

        public static async Task<GivenAnswer> GetById(long id) { return await App.Networking.Get<GivenAnswer>($"api/groups/{id}"); }
        #endregion
    }
}