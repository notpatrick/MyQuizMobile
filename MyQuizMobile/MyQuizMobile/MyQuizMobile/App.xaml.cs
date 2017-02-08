using MYQuizMobile;
using NLog;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MyQuizMobile {
    public partial class App : Application {
        public static Networking Networking;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public App() {
            InitializeComponent();
            MainPage = new RootPage();
        }

        protected override async void OnStart() {
#if !DEBUG
            await MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage()), false);
#else
            Current.Properties["DeviceID"] = 1;
            Networking = new Networking(Current.Properties["DeviceID"].ToString());
#endif
            logger.Info($"OnStart with DeviceID {Current.Properties["DeviceID"]}");
        }

        protected override async void OnSleep() {
            await Current.SavePropertiesAsync();
            base.OnSleep();
        }
    }
}