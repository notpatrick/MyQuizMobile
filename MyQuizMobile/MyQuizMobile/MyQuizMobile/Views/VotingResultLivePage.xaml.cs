using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VotingResultLivePage : ContentPage {
        public VotingResultLiveViewModel VotingResultLiveViewModel;

        public VotingResultLivePage(VotingStartViewModel asvm) {
            InitializeComponent();
            VotingResultLiveViewModel = new VotingResultLiveViewModel(asvm);
            BindingContext = VotingResultLiveViewModel;

            weiterButton.Clicked += VotingResultLiveViewModel.weiterButton_Clicked;
            timeEntry.Focused += VotingResultLiveViewModel.timeEntry_OnFocused;
            timeEntry.Unfocused += VotingResultLiveViewModel.timeEntry_OnUnfocused;
            listView.ItemTapped += VotingResultLiveViewModel.listView_ItemTapped;
            personPicker.ItemSelected += VotingResultLiveViewModel.personPicker_ItemSelected;
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override bool OnBackButtonPressed() { return VotingResultLiveViewModel.OnBackButtonPressed(this); }
    }
}