using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public class Group : MenuItem {
        public override int Id { get; set; }
        public string Title { get; set; }
        public string EnterGroupPin { get; set; }
        [JsonIgnore]
        public ObservableCollection<SingleTopic> SingleTopics { get; set; } = new ObservableCollection<SingleTopic>();
        public override string DisplayText { get { return Title; } set { Title = value; } }
        public override ItemType ItemType => ItemType.Group;
        public override string DetailText { get { return EnterGroupPin; } set { EnterGroupPin = value; } }

        #region POST
        public static async Task<Group> Post(Group g) {
            var group = await App.Networking.Post("api/groups/", g);
            var singletopics = await App.Networking.Post($"api/groups/{group.Id}/topics", g.SingleTopics);
            group.SingleTopics = singletopics;
            return group;
        }
        #endregion

        #region DELETE
        public static async void DeleteById(int i) { await App.Networking.Delete($"api/groups/{i}"); }
        #endregion

        #region GET
        public static async Task<List<Group>> GetAll() { return await App.Networking.Get<List<Group>>("api/groups/"); }
        public static async Task<Group> GetById(int i) { return await App.Networking.Get<Group>($"api/groups/{i}"); }
        #endregion
    }
}