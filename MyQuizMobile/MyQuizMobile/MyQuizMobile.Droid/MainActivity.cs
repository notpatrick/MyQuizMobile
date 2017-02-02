using Android.App;
using Android.Content.PM;
using Android.OS;

[assembly: Application(Icon = "@drawable/Icon", Theme = "@android:style/Theme.Holo.Light")]
namespace MyQuizMobile.Droid
{
	[Activity (Label = "MyQuizMobile", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            global::Xamarin.Forms.Forms.Init (this, bundle);
            LoadApplication (new MyQuizMobile.App ());
            
            var x = typeof(Xamarin.Forms.Themes.LightThemeResources);
            x = typeof(Xamarin.Forms.Themes.DarkThemeResources);
            x = typeof(Xamarin.Forms.Themes.Android.UnderlineEffect);
        }
	}
}

