using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class SideMenuPage : ContentPage {
        private readonly SideMenuViewModel SideMenuViewModel;

        public SideMenuPage() {
            InitializeComponent();
            SideMenuViewModel = new SideMenuViewModel();
            BindingContext = SideMenuViewModel;
            listView.ItemSelected += (s, e) => { listView.SelectedItem = null; };
        }
    }
}