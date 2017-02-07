using System.Threading.Tasks;
using System.Windows.Input;
using MYQuizMobile;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class LoginViewModel {
        private const string Password = "1337";
        public string InputText { get; set; }

        public ICommand ButtonClickCommand { get; set; }

        public LoginViewModel() { ButtonClickCommand = new Command(async () => { await Login(); }); }

        private async Task Login() {
            // TODO: UI lags on button press
            if (InputText == Password) {
                if (!IsRegistered()) {
                    var id = await Register(InputText);
                    App.Networking = new Networking(id);
                } else {
                    App.Networking = new Networking(Application.Current.Properties["DeviceID"].ToString());
                }

                await Application.Current.MainPage.Navigation.PopModalAsync(true);
            } else {
                MessagingCenter.Send(this, "WrongPassword");
            }
        }

        private bool IsRegistered() {
            int id;
            int.TryParse(Application.Current.Properties["DeviceID"].ToString(), out id);
            return id != 0;
        }

        private async Task<string> Register(string password) {
            var regDevice = await App.Networking.Post("api/devices/", new {Password = password, DeviceId = 0, Id = 0});
            Application.Current.Properties["DeviceID"] = regDevice.Id;
            await Application.Current.SavePropertiesAsync();
            return Application.Current.Properties["DeviceID"].ToString();
        }
    }
}