using System.Threading.Tasks;
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

        protected override void OnStart() {
            Networking = new Networking();
            Task.Run(async () => { await Networking.Get<string>($"api/devices/{1}"); });
            // TODO: Device authentication here
        }
    }
}