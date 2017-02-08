using MYQuizMobile;
using NLog;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MyQuizMobile {
    public partial class App : Application {
        public static Networking Networking = new Networking("");
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
                logger.Info("OnStart without DeviceID");
            } else {
                logger.Info($"OnStart with DeviceID {Current.Properties["DeviceID"]}");
            }
        }
    }
}