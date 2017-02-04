using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyQuizMobile.DataModel {
    public partial class Device : Item {
        [JsonIgnore]
        public bool IsAdminBool { get { return IsAdmin == 1; } set { IsAdmin = value ? 1 : 0; } }
        public override string DisplayText => $"{Id}";
        public override string DetailText => IsAdminBool ? "Admin" : "Client";
        public override ItemType ItemType => ItemType.Device;

        #region POST
        public static async Task<Device> Post(Device device) {
            return await App.Networking.Post("api/groups/", device);
        }
        #endregion

        #region DELETE
        public static async void DeleteById(int id) { await App.Networking.Delete($"api/groups/{id}"); }
        #endregion

        #region GET
        public static async Task<List<Device>> GetAll() {
            return await App.Networking.Get<List<Device>>("api/groups/");
        }

        public static async Task<Device> GetById(int id) {
            return await App.Networking.Get<Device>($"api/groups/{id}");
        }
        #endregion
    }
}