using MYQuizMobile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MyQuizMobile {
    public partial class App : Application {
        public static Networking Networking;

        public App() {
            InitializeComponent();
            MainPage = new RootView();
        }

        protected override void OnStart() { Networking = new Networking(); }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }
}