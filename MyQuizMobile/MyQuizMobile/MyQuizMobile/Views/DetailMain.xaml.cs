using System;
using OkHttp;
using Xamarin.Forms;

namespace MyQuizMobile
{
	public partial class DetailMain : ContentPage
    {
	    public DetailMainViewModel DetailMainViewModel;

		public DetailMain ()
		{
			InitializeComponent ();
		    DetailMainViewModel = new DetailMainViewModel(this);
		    BindingContext = DetailMainViewModel;
		    EntryVeranstaltung.Focused += EntryVeranstaltunen_Tapped;
		}
        // TODO: create custom eventargs to pass type and make this generic
        public void EntryVeranstaltunen_Tapped(object sender, EventArgs e)
        {
            DetailMainViewModel.OnVeranstaltungTap(sender, e);
            EntryVeranstaltung.Focused -= EntryVeranstaltunen_Tapped;
        }

        // TODO: create custom eventargs to pass type and make this generic
        public async void SetEntryVeranstaltung(object sender, EventArgs e)
        {
            await RootView.rootNavigationPage.Navigation.PopAsync();
            EntryVeranstaltung.Text = (e as StringEventArg)?.Name;
            EntryVeranstaltung.Focused += EntryVeranstaltunen_Tapped;
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
