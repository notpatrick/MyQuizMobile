using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VotingSelectionPage : ContentPage {
        public VotingSelectionViewModel VotingSelectionViewModel;

        public VotingSelectionPage(Item item) {
            InitializeComponent();
            VotingSelectionViewModel = new VotingSelectionViewModel(item);
            BindingContext = VotingSelectionViewModel;
        }

        protected override void OnAppearing() {
            MessagingCenter.Unsubscribe<VotingSelectionViewModel>(this, "Selected");
            MessagingCenter.Subscribe<VotingSelectionViewModel>(this, "Selected",
                                                                sender => { listView.SelectedItem = null; });

            base.OnAppearing();
        }

        protected override void OnDisappearing() {
            MessagingCenter.Unsubscribe<VotingSelectionViewModel>(this, "Selected");
            base.OnDisappearing();
        }
    }
}