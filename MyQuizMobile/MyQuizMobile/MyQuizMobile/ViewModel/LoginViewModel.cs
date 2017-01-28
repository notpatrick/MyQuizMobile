using MyQuizMobile.Helpers;
using MyQuizMobile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MyQuizMobile.Services;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;

namespace MyQuizMobile.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            SignInCommand = new Command(async () => await SignIn());
            NotNowCommand = new Command(App.GoToMainPage);
        }

        string message = string.Empty;
        public string Message
        {
            get { return message; }
            set { message = value;  OnPropertyChanged(); }
        }

        public ICommand NotNowCommand { get; }

        public ICommand SignInCommand { get; }
        async Task SignIn()
        {
            try
            {
                IsBusy = true;
                Message = "Signing In...";

                await TryLoginAsync(StoreManager);

            }
            finally
            {

                Message = string.Empty;
                IsBusy = false;

                if (Settings.IsLoggedIn)
                    App.GoToMainPage();
            }
        }

        public static Task<bool> TryLoginAsync(IStoreManager storeManager)
        {
            return Task.FromResult(true);
        }
    }
}
