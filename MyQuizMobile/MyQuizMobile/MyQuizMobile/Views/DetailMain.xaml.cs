using System;
using OkHttp;
using Xamarin.Forms;

namespace MyQuizMobile
{
	public partial class DetailMain : ContentPage
    {
	    public DetailMainViewModel DetailMainViewModel;
        public TapGestureRecognizer TapGestureRecognizer;

		public DetailMain ()
		{
			InitializeComponent ();
		    DetailMainViewModel = new DetailMainViewModel(this);
		    BindingContext = DetailMainViewModel;
		    EntryVeranstaltung.Focused += EntryVeranstaltunen_Tapped;
		}

        public void EntryVeranstaltunen_Tapped(object sender, EventArgs e)
        {
            DetailMainViewModel.OnVeranstaltungTap(sender, e);
        }

        public async void SetEntryVeranstaltung(object sender, EventArgs e)
        {
            await RootView.rootNavigationPage.Navigation.PopAsync();
            EntryVeranstaltung.Text = (e as StringEventArg)?.Name;
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
