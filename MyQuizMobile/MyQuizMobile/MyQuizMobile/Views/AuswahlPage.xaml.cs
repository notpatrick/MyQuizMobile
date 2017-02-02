using Xamarin.Forms;

namespace MyQuizMobile
{

    public partial class AuswahlPage : ContentPage
    {
        public AuswahlViewModel AuswahlViewModel;

        public AuswahlPage(MenuItem item)
        {
            InitializeComponent();
            AuswahlViewModel = new AuswahlViewModel(item);
            BindingContext = AuswahlViewModel;

            listAuswahl.ItemSelected += AuswahlViewModel.OnItemSelected;
        }
    }
}
