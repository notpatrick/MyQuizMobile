using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VotingResultLivePage : ContentPage {
        public VotingResultLiveViewModel VotingResultLiveViewModel;

        public VotingResultLivePage(VotingStartViewModel asvm) {
            InitializeComponent();
            VotingResultLiveViewModel = new VotingResultLiveViewModel(asvm);
            BindingContext = VotingResultLiveViewModel;
            NavigationPage.SetHasBackButton(this, false);
            resultListView.ItemSelected += (sender, args) => { ((ListView)sender).SelectedItem = null; };
        }

        protected override bool OnBackButtonPressed() { return VotingResultLiveViewModel.OnBackButtonPressed(this); }
    }
}