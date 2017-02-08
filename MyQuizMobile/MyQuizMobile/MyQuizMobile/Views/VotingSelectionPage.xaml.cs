using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VotingSelectionPage : ContentPage {
        public VotingSelectionViewModel VotingSelectionViewModel;

        public VotingSelectionPage(Item item) {
            InitializeComponent();
            VotingSelectionViewModel = new VotingSelectionViewModel(item);
            BindingContext = VotingSelectionViewModel;

            listView.ItemSelected += (s, e) => { listView.SelectedItem = null; };
        }

    }
}