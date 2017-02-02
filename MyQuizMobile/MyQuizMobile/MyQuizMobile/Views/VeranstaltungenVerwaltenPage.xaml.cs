using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VeranstaltungenVerwaltenPage : ContentPage {
        public VeranstaltungenVerwaltenViewModel VeranstaltungenVerwaltenViewModel;

        public VeranstaltungenVerwaltenPage() {
            InitializeComponent();
            VeranstaltungenVerwaltenViewModel = new VeranstaltungenVerwaltenViewModel();
            BindingContext = VeranstaltungenVerwaltenViewModel;

            addButton.Clicked += VeranstaltungenVerwaltenViewModel.addButton_Clicked;
        }
    }
}