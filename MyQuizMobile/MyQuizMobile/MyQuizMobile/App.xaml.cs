using MYQuizMobile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MyQuizMobile {
    public partial class App : Application {
        public static Networking Networking;

        public App() {
            InitializeComponent();
#if DEBUG
            MainPage = new RootPage();
#else
            MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage()), false);
#endif
        }

        protected override async void OnStart() {
            Current.Properties.Remove("DeviceID");
            // TODO: This is one time authentication without password entry
            if (!Current.Properties.ContainsKey("DeviceID")) {
                var n = new Networking("");
                var regDevice = await n.Post("api/devices/", new {
                    Password = "1337",
                    DeviceId = 0,
                    Id = 0
                });
                Current.Properties["DeviceID"] = regDevice.Id;
                await Current.SavePropertiesAsync();
                Networking = new Networking(Current.Properties["DeviceID"].ToString());
            } else {
                Networking = new Networking(Current.Properties["DeviceID"].ToString());
            }
        }
    }
}