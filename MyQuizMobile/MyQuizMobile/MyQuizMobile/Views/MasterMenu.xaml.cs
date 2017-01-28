using Xamarin.Forms;

namespace MyQuizMobile
{
	public partial class MasterMenu : ContentPage
	{
	    public MasterMenuViewModel MasterMenuViewModel;
		public MasterMenu ()
		{
			InitializeComponent();
		    MasterMenuViewModel = new MasterMenuViewModel();
		    BindingContext = MasterMenuViewModel;

		    listView.ItemSelected += MasterMenuViewModel.OnItemSelected;
		}
	}
}
