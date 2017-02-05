using System;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class LoginViewModel {
        private const string Password = "1337";

        public string InputText { get; set; }

        public void loginButton_Clicked(object sender, EventArgs e) {
            if (InputText == Password) {
                Application.Current.MainPage.Navigation.PopModalAsync(true);
            } else {
                MessagingCenter.Send(this, "WrongPassword");
            }
        }

        public void DisplayAlert(ContentPage page) {
            Device.BeginInvokeOnMainThread(async () => {
                await page.DisplayAlert("Achtung!", "Passwort falsch",
                                        "Ok");
            });
        }
    }
}