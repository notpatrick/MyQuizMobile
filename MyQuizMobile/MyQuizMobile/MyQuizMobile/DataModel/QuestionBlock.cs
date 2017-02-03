using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public class QuestionBlock : Item {
        public override int Id { get; set; }
        public string Title { get; set; }
        [JsonIgnore]
        public List<Question> Questions { get; set; }
        public override string DisplayText { get { return Title; } set { Title = value; } }
        public override ItemType ItemType => ItemType.QuestionBlock;
        public override string DetailText => Questions != null ? $"{Questions.Count} Fragen" : string.Empty;

        #region POST
        public static async Task<QuestionBlock> Post(QuestionBlock questionBlock) {
            return await App.Networking.Post("api/questionBlock/", questionBlock);
        }
        #endregion

        #region DELETE
        public static async void DeleteById(int id) { await App.Networking.Delete($"api/questionBlock/{id}"); }
        #endregion

        #region GET
        public static async Task<List<QuestionBlock>> GetAll() {
            return await App.Networking.Get<List<QuestionBlock>>("api/questionBlock/");
        }

        public static async Task<QuestionBlock> GetById(int id) {
            return await App.Networking.Get<QuestionBlock>($"api/questionBlock/{id}");
        }
        #endregion
    }
}