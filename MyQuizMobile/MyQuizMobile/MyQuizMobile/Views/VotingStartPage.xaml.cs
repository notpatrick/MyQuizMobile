using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VotingStartPage : ContentPage {
        public VotingStartViewModel VotingStartViewModel;

        public VotingStartPage() {
            InitializeComponent();
            VotingStartViewModel = new VotingStartViewModel();
            BindingContext = VotingStartViewModel;
        }

        protected override void OnAppearing() {
            MessagingCenter.Unsubscribe<VotingStartViewModel>(this, "Selected");
            MessagingCenter.Subscribe<VotingStartViewModel>(this, "Selected",
                                                            sender => { listView.SelectedItem = null; });
            base.OnAppearing();
        }

        protected override void OnDisappearing() {
            MessagingCenter.Unsubscribe<VotingStartViewModel>(this, "Selected");
            base.OnDisappearing();
        }
    }
}