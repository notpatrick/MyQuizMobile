using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class AuswahlPage : ContentPage {
        public AuswahlViewModel AuswahlViewModel;

        public AuswahlPage(MenuItem item) {
            InitializeComponent();
            AuswahlViewModel = new AuswahlViewModel(item);
            BindingContext = AuswahlViewModel;

            listAuswahl.ItemSelected += AuswahlViewModel.OnItemSelected;

            listAuswahl.Refreshing += AuswahlViewModel.listView_Refreshing;
            searchBar.TextChanged += AuswahlViewModel.searchBar_TextChanged;
            Appearing += AuswahlViewModel.OnAppearing;
        }
        protected override void OnDisappearing()
        {
            listAuswahl.SelectedItem = null;
            base.OnDisappearing();
        }
    }
}