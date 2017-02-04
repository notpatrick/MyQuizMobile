using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public class Group : Item {
        public override int Id { get; set; }
        public string Title { get; set; }
        public string EnterGroupPin { get; set; }

        public ObservableCollection<SingleTopic> topicList { get; set; } = new ObservableCollection<SingleTopic>();
        public override string DisplayText { get { return Title; } set { Title = value; } }
        public override ItemType ItemType => ItemType.Group;
        public override string DetailText { get { return EnterGroupPin; } set { EnterGroupPin = value; } }

        #region POST
        public static async Task<Group> Post(Group grou) {
            // TODO: Merge these when backend can write children recursively
            var resultGroup = await App.Networking.Post("api/groups/", grou);
            var resultSingleTopics = await App.Networking.Post($"api/groups/{resultGroup.Id}/topics", grou.topicList);
            resultGroup.topicList = resultSingleTopics;
            return resultGroup;
        }
        #endregion

        #region DELETE
        public static async void DeleteById(int id) { await App.Networking.Delete($"api/groups/{id}"); }
        #endregion

        #region GET
        public static async Task<List<Group>> GetAll() { return await App.Networking.Get<List<Group>>("api/groups/"); }
        public static async Task<Group> GetById(int id) { return await App.Networking.Get<Group>($"api/groups/{id}"); }
        #endregion
    }
}