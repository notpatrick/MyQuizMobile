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
            MessagingCenter.Unsubscribe<VotingSelectionViewModel>(this, "PickDone");
            MessagingCenter.Subscribe<VotingSelectionViewModel>(this, "PickDone", sender => { listView.SelectedItem = null; });

            base.OnAppearing();
        }

        protected override void OnDisappearing() {
            MessagingCenter.Unsubscribe<VotingSelectionViewModel>(this, "PickDone");
            base.OnDisappearing();
        }
    }
}