using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VotingResultLivePage : ContentPage {
        public VotingResultLiveViewModel VotingResultLiveViewModel;

        public VotingResultLivePage(VotingStartViewModel asvm) {
            InitializeComponent();
            VotingResultLiveViewModel = new VotingResultLiveViewModel(asvm);
            BindingContext = VotingResultLiveViewModel;
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override bool OnBackButtonPressed() { return VotingResultLiveViewModel.OnBackButtonPressed(this); }
    }
}