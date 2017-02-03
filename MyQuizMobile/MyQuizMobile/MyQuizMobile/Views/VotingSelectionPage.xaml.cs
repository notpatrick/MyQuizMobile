using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VotingSelectionPage : ContentPage {
        public VotingSelectionViewModel VotingSelectionViewModel;

        public VotingSelectionPage(Item item) {
            InitializeComponent();
            VotingSelectionViewModel = new VotingSelectionViewModel(item);
            BindingContext = VotingSelectionViewModel;

            listAuswahl.ItemSelected += VotingSelectionViewModel.OnItemSelected;

            listAuswahl.Refreshing += VotingSelectionViewModel.listView_Refreshing;
            searchBar.TextChanged += VotingSelectionViewModel.searchBar_TextChanged;
            Appearing += VotingSelectionViewModel.OnAppearing;
        }

        protected override void OnDisappearing() {
            listAuswahl.SelectedItem = null;
            base.OnDisappearing();
        }
    }
}