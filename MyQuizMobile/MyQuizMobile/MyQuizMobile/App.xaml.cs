using MYQuizMobile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MyQuizMobile {
    public partial class App : Application {
        public static Networking Networking = new Networking("");
        public static Socket Socket = new Socket();

        public App() {
            InitializeComponent();
            MainPage = new RootPage();
        }

        protected override async void OnStart() {
#if !DEBUG
            await MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage()), false);
#endif
            if (!Current.Properties.ContainsKey("DeviceID")) {
                Current.Properties["DeviceID"] = 0;
            }
        }
    }
}