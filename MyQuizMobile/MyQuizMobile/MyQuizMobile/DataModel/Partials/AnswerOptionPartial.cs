using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyQuizMobile.DataModel {
    public partial class AnswerOption : Item {
        public override string DisplayText { get { return Text; } set { Text = value; } }
        public override ItemType ItemType => ItemType.AnswerOption;

        #region POST
        public static async Task<AnswerOption> Post(AnswerOption answerOption) {
            return await App.Networking.Post("api/groups/", answerOption);
        }
        #endregion

        #region DELETE
        public static async void DeleteById(int id) { await App.Networking.Delete($"api/groups/{id}"); }
        #endregion

        #region GET
        public static async Task<List<AnswerOption>> GetAll() {
            return await App.Networking.Get<List<AnswerOption>>("api/groups/");
        }

        public static async Task<AnswerOption> GetById(int id) {
            return await App.Networking.Get<AnswerOption>($"api/groups/{id}");
        }
        #endregion
    }
}