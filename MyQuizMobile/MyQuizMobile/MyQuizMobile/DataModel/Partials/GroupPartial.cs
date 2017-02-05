using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PostSharp.Patterns.Model;

namespace MyQuizMobile.DataModel {
    [NotifyPropertyChanged]
    public partial class Group : Item {
        public ObservableCollection<SingleTopic> SingleTopics { get; set; } = new ObservableCollection<SingleTopic>();
        public override string DisplayText { get { return Title; } set { Title = value; } }
        public override ItemType ItemType => ItemType.Group;
        public override string DetailText => $"{EnterGroupPin}";

        #region POST
        public static async Task<Group> Post(Group group) { return await App.Networking.Post("api/groups/", group); }
        #endregion

        #region DELETE
        public static async Task DeleteById(int id) { await App.Networking.Delete($"api/groups/{id}"); }
        #endregion

        #region GET
        public static async Task<List<Group>> GetAll() { return await App.Networking.Get<List<Group>>("api/groups/"); }
        public static async Task<Group> GetById(int id) { return await App.Networking.Get<Group>($"api/groups/{id}"); }
        #endregion
    }
}