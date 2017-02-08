using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Support.V7.App;

namespace MyQuizMobile.Droid {
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity {
        protected override void OnResume() {
            base.OnResume();

            var task = new Task(t => { StartActivity(new Intent(Application.Context, typeof(MainActivity))); }, TaskScheduler.FromCurrentSynchronizationContext());
            task.Start();
        }
    }
}