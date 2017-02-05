using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class LoginPage : ContentPage {
        public LoginViewModel LoginViewModel;

        public LoginPage() {
            InitializeComponent();
            LoginViewModel = new LoginViewModel();
            BindingContext = LoginViewModel;
        }

        private void Popup() {
            Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Achtung!", "Passwort falsch", "Ok"); });
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            MessagingCenter.Unsubscribe<LoginViewModel>(this, "WrongPassword");
            MessagingCenter.Subscribe<LoginViewModel>(this, "WrongPassword", sender => Popup());
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LoginViewModel>(this, "WrongPassword");
        }

        protected override bool OnBackButtonPressed() { return true; }
    }
}