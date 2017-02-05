using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class LoginPage : ContentPage {
        public LoginViewModel LoginViewModel;

        public LoginPage() {
            InitializeComponent();
            LoginViewModel = new LoginViewModel();
            BindingContext = LoginViewModel;
            loginButton.Clicked += LoginViewModel.loginButton_Clicked;
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            MessagingCenter.Unsubscribe<LoginViewModel>(this, "WrongPassword");
            MessagingCenter.Subscribe<LoginViewModel>(this, "WrongPassword",
                                                      sender => { LoginViewModel.DisplayAlert(this); });
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<LoginViewModel>(this, "WrongPassword");
        }

        protected override bool OnBackButtonPressed() { return true; }
    }
}