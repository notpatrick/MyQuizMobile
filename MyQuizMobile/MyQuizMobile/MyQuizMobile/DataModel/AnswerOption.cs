using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyQuizMobile.DataModel {
    public class AnswerOption : MenuItem {
        public override int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public override string DisplayText { get { return Text; } set { Text = value; } }
        public override ItemType ItemType => ItemType.AnswerOption;


        #region POST
        public static async Task<AnswerOption> Post(AnswerOption g)
        {
            var group = await App.Networking.Post("api/groups/", g);
            return group;
        }
        #endregion

        #region DELETE
        public static async void DeleteById(int i) { await App.Networking.Delete($"api/groups/{i}"); }
        #endregion

        #region GET
        public static async Task<List<AnswerOption>> GetAll() { return await App.Networking.Get<List<AnswerOption>>("api/groups/"); }
        public static async Task<AnswerOption> GetById(int i) { return await App.Networking.Get<AnswerOption>($"api/groups/{i}"); }
        #endregion
    }
}