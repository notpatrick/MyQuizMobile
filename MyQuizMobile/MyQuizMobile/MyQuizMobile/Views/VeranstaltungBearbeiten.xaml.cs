using MyQuizMobile.DataModel;
using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class VeranstaltungBearbeiten : ContentPage {
        public VeranstaltungBearbeitenViewModel VeranstaltungBearbeitenViewModel;

        public VeranstaltungBearbeiten(Group group) {
            InitializeComponent();
            VeranstaltungBearbeitenViewModel = new VeranstaltungBearbeitenViewModel(group);
            BindingContext = VeranstaltungBearbeitenViewModel;
            abbrechenButton.Clicked += VeranstaltungBearbeitenViewModel.abbrechenButton_Clicked;
            saveButton.Clicked += VeranstaltungBearbeitenViewModel.saveButton_Clicked;
            löschenButton.Clicked += VeranstaltungBearbeitenViewModel.löschenButton_Clicked;
            Appearing += VeranstaltungBearbeitenViewModel.OnAppearing;
        }
    }
}