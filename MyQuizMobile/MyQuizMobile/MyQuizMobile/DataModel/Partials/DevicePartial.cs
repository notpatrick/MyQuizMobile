using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyQuizMobile.DataModel {
    public partial class Device : Item {
        public override string DisplayText => $"{Id}";
        public override string DetailText => IsAdmin == 1 ? "Admin" : "Client";
        public override ItemType ItemType => ItemType.Device;

        #region POST
        public static async Task<Device> Post(Device device) { return await App.Networking.Post("api/groups/", device); }
        #endregion

        #region DELETE
        public static async void DeleteById(long id) { await App.Networking.Delete($"api/groups/{id}"); }
        #endregion

        #region GET
        public static async Task<List<Device>> GetAll() { return await App.Networking.Get<List<Device>>("api/groups/"); }

        public static async Task<Device> GetById(long id) { return await App.Networking.Get<Device>($"api/groups/{id}"); }
        #endregion
    }
}