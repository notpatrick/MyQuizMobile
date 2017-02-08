using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public class GivenAnswer : Item {
        public QuestionBlock QuestionBlock { get; set; }
        public Question Question { get; set; }
        public Group Group { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SingleTopic SingleTopic { get; set; }

        [DefaultValue(0)]
        public long SurveyId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TimeStamp { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AnswerOption AnswerOption { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Device Device { get; set; }

        public override long Id { get; set; }
        public override string DisplayText => $"Given Answer {Id}";
        public override ItemType ItemType => ItemType.GivenAnswer;

        #region DELETE
        public static async void DeleteById(long id) { await App.Networking.Delete($"api/groups/{id}"); }
        #endregion

        #region POST
        public static async Task<GivenAnswer> Post(GivenAnswer givenAnswer) { return await App.Networking.Post("api/groups/", givenAnswer); }
        public static async Task<List<GivenAnswer>> Start(List<GivenAnswer> givenAnswers, long timeInSeconds) { return await App.Networking.Post($"api/givenanswer/start/{timeInSeconds}", givenAnswers); }
        #endregion

        #region GET
        public static async Task<List<GivenAnswer>> GetAll() { return await App.Networking.Get<List<GivenAnswer>>("api/groups/"); }

        public static async Task<GivenAnswer> GetById(long id) { return await App.Networking.Get<GivenAnswer>($"api/groups/{id}"); }
        #endregion
    }
}