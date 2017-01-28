//#define FlyoutNavigation

using MyQuizMobile.Helpers;
using MyQuizMobile.Interfaces;
using MyQuizMobile.View;

using System.Collections.Generic;
using Xamarin.Forms;

namespace MyQuizMobile
{
    public partial class App : Application
    {

        public App()
        {
            // Loads all reasources from App.xaml
            InitializeComponent();

            if (DependencyService.Get<IStoreManager>().UseAuth && Settings.IsLoggedIn)
                GoToMainPage();
            else
                GoToLogin();
        }

        public static void GoToMainPage()
        {

#if FlyoutNavigation
            Current.MainPage = new RootMasterDetailPage();
#else

            // The root page of your application
            switch (Device.OS)
            {
                case TargetPlatform.iOS:
                    Current.MainPage = new MainTabPageiOS();
                    break;
                default:
                    Current.MainPage = new NavigationPage(new MainTabPage())
                    {
                        BarBackgroundColor = (Color)Current.Resources["Primary"],
                        BarTextColor = Color.White
                    };
                    break;
            }
#endif
        }

        public static void GoToLogin()
        {
            Current.MainPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = (Color)Current.Resources["Primary"],
                BarTextColor = Color.White
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            MessagingCenter.Subscribe<MessagingCenterAlert>(this, "message", async (info) =>
            {
                var task = Current?.MainPage?.DisplayAlert(info.Title, info.Message, info.Cancel);

                if (task == null)
                    return;

                await task;
                info?.OnCompleted?.Invoke();
            });
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
 
