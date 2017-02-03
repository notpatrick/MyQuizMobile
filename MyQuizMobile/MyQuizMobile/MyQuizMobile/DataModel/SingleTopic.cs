using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public class SingleTopic : MenuItem {
        public override int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateTime { get; set; }
        public override string DisplayText { get { return Name; } set { Name = value; } }
        public override ItemType ItemType => ItemType.SingleTopic;
        [JsonIgnore]
        public bool UmfrageDone { get; set; } = false;


        #region POST
        public static async Task<SingleTopic> Post(SingleTopic g)
        {
            var group = await App.Networking.Post("api/groups/", g);
            return group;
        }
        #endregion

        #region DELETE
        public static async void DeleteById(int i) { await App.Networking.Delete($"api/groups/{i}"); }
        #endregion

        #region GET
        public static async Task<List<SingleTopic>> GetAll() { return await App.Networking.Get<List<SingleTopic>>("api/groups/"); }
        public static async Task<SingleTopic> GetById(int i) { return await App.Networking.Get<SingleTopic>($"api/groups/{i}"); }
        #endregion
    }
}