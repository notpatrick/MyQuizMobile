using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VotingStartPage : ContentPage {
        public VotingStartViewModel VotingStartViewModel;

        public VotingStartPage() {
            InitializeComponent();
            VotingStartViewModel = new VotingStartViewModel();
            BindingContext = VotingStartViewModel;

            listView.ItemSelected += VotingStartViewModel.OnMenuItemTapped;
            timeEntry.Focused += VotingStartViewModel.timeEntry_OnFocused;
            timeEntry.Unfocused += VotingStartViewModel.timeEntry_OnUnfocused;
            weiterButton.Clicked += VotingStartViewModel.continueButton_Clicked;
            personenbezogenSwitch.Toggled += VotingStartViewModel.isPersonalSwitch_Toggled;
        }

        protected override void OnDisappearing() {
            listView.SelectedItem = null;
            base.OnDisappearing();
        }
    }
}