using Xamarin.Forms;

namespace MyQuizMobile
{
	public partial class LiveResultPage : ContentPage
	{
	    public LiveResultViewModel LiveResultViewModel;
		public LiveResultPage ()
		{
			InitializeComponent ();
            LiveResultViewModel = new LiveResultViewModel();
		    BindingContext = LiveResultViewModel;
		    weiterButton.Clicked += LiveResultViewModel.weiterButton_Clicked;
		}
	}
}
