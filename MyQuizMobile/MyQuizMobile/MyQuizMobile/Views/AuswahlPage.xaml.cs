using Xamarin.Forms;

namespace MyQuizMobile
{

    public partial class AuswahlPage : ContentPage
    {
        public AuswahlViewModel AuswahlViewModel;

        public AuswahlPage(IMenuItem item)
        {
            InitializeComponent();
            AuswahlViewModel = new AuswahlViewModel(item);
            BindingContext = AuswahlViewModel;

            listAuswahl.ItemSelected += AuswahlViewModel.OnItemSelected;
        }
    }
}
