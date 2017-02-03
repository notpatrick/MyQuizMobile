using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyQuizMobile.DataModel {
    public class GivenAnswer : MenuItem {
        public DateTime TimeStamp { get; set; }
        public QuestionBlock QuestionBlock { get; set; }
        public Question Question { get; set; }
        public AnswerOption AnswerOption { get; set; }
        public SingleTopic SingleTopic { get; set; }
        public Group Group { get; set; }
        public Device Device { get; set; }
        public override int Id { get; set; }
        public override string DisplayText => $"Given Answer {Id}";
        public override ItemType ItemType => ItemType.GivenAnswer;

        #region POST
        public static async Task<GivenAnswer> Post(GivenAnswer g) {
            var group = await App.Networking.Post("api/groups/", g);
            return group;
        }
        #endregion

        #region DELETE
        public static async void DeleteById(int i) { await App.Networking.Delete($"api/groups/{i}"); }
        #endregion

        #region GET
        public static async Task<List<GivenAnswer>> GetAll() {
            return await App.Networking.Get<List<GivenAnswer>>("api/groups/");
        }

        public static async Task<GivenAnswer> GetById(int i) {
            return await App.Networking.Get<GivenAnswer>($"api/groups/{i}");
        }
        #endregion
    }

    public class IdGivenAnswer {
        public DateTime? TimeStamp { get; set; }
        public int? QuestionBlockId { get; set; }
        public int QuestionId { get; set; }
        public int? AnswerOptionId { get; set; }
        public int? SingleTopicId { get; set; }
        public int GroupId { get; set; }
        public int? DeviceId { get; set; }

        public IdGivenAnswer(GivenAnswer ga) {
            GroupId = ga.Group.Id;
            QuestionId = ga.Question.Id;
            QuestionBlockId = ga.QuestionBlock?.Id;
            SingleTopicId = ga.SingleTopic?.Id;
        }
    }
}