using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VotingStartPage : ContentPage {
        public VotingStartViewModel VotingStartViewModel;

        public VotingStartPage() {
            InitializeComponent();
            VotingStartViewModel = new VotingStartViewModel();
            BindingContext = VotingStartViewModel;
            listView.ItemSelected += (s, e) => { listView.SelectedItem = null; };
        }
    }
}