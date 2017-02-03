using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VeranstaltungenVerwaltenPage : ContentPage {
        public VeranstaltungenVerwaltenViewModel VeranstaltungenVerwaltenViewModel;

        public VeranstaltungenVerwaltenPage() {
            InitializeComponent();
            VeranstaltungenVerwaltenViewModel = new VeranstaltungenVerwaltenViewModel();
            BindingContext = VeranstaltungenVerwaltenViewModel;

            addButton.Clicked += VeranstaltungenVerwaltenViewModel.addButton_Clicked;
            listView.Refreshing += VeranstaltungenVerwaltenViewModel.listView_Refreshing;
            searchBar.TextChanged += VeranstaltungenVerwaltenViewModel.searchBar_TextChanged;
            listView.ItemSelected += VeranstaltungenVerwaltenViewModel.OnMenuItemTapped;
            Appearing += VeranstaltungenVerwaltenViewModel.OnAppearing;
        }
    }
}