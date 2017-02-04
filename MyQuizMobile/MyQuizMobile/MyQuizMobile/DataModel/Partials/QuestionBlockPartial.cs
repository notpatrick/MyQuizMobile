using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyQuizMobile.DataModel {
    public partial class QuestionBlock : Item {
        public ObservableCollection<Question> questionList { get; set; } = new ObservableCollection<Question>();
        public override string DisplayText { get { return Title; } set { Title = value; } }
        public override ItemType ItemType => ItemType.QuestionBlock;
        public override string DetailText => questionList.Count > 0 ? $"{questionList.Count} Fragen" : string.Empty;

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