using Xamarin.Forms;

namespace MyQuizMobile
{
    public partial class SideMenuPage : ContentPage
    {
        private SideMenuViewModel SideMenuViewModel;

        public SideMenuPage()
        {
            InitializeComponent();
            SideMenuViewModel = new SideMenuViewModel();
            BindingContext = SideMenuViewModel;

            listView.ItemSelected += SideMenuViewModel.OnItemSelected;
        }
    }
}
