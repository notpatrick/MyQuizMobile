using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Themes;
using Xamarin.Forms.Themes.Android;

[assembly: Application(Icon = "@drawable/Icon", Theme = "@android:style/Theme.Holo.Light")]

namespace MyQuizMobile.Droid {
    [Activity(Label = "MyQuizMobile", Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            LoadApplication(new App());

            var x = typeof(LightThemeResources);
            x = typeof(DarkThemeResources);
            x = typeof(UnderlineEffect);
        }
    }
}