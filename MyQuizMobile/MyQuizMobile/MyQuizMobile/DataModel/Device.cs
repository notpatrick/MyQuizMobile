﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyQuizMobile.DataModel {
    public class Device : MenuItem {
        public override int Id { get; set; }
        public string PushUpToken { get; set; }
        public bool IsAdmin { get; set; }
        public override string DisplayText => $"{Id} - {IsAdmin}";
        public override ItemType ItemType { get; set; }


        #region POST
        public static async Task<Device> Post(Device g)
        {
            var group = await App.Networking.Post("api/groups/", g);
            return group;
        }
        #endregion

        #region DELETE
        public static async void DeleteById(int i) { await App.Networking.Delete($"api/groups/{i}"); }
        #endregion

        #region GET
        public static async Task<List<Device>> GetAll() { return await App.Networking.Get<List<Device>>("api/groups/"); }
        public static async Task<Device> GetById(int i) { return await App.Networking.Get<Device>($"api/groups/{i}"); }
        #endregion
    }
}