using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class SideMenuPage : ContentPage {
        private readonly SideMenuViewModel SideMenuViewModel;

        public SideMenuPage() {
            InitializeComponent();
            SideMenuViewModel = new SideMenuViewModel();
            BindingContext = SideMenuViewModel;
        }

        protected override void OnAppearing() {
            MessagingCenter.Unsubscribe<SideMenuViewModel>(this, "Selected");
            MessagingCenter.Subscribe<SideMenuViewModel>(this, "Selected", sender => { listView.SelectedItem = null; });
            base.OnAppearing();
        }

        protected override void OnDisappearing() {
            MessagingCenter.Unsubscribe<SideMenuViewModel>(this, "Selected");
            base.OnDisappearing();
        }
    }
}