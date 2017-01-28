using Xamarin.Forms;

namespace MyQuizMobile
{
	public partial class MasterMenu : ContentPage
	{
	    public MasterMenuViewModel MasterMenuViewModel;
		public MasterMenu ()
		{
			InitializeComponent();
		    MasterMenuViewModel = DependencyService.Get<MasterMenuViewModel>(DependencyFetchTarget.GlobalInstance);
		    BindingContext = MasterMenuViewModel;

		    listView.ItemSelected += MasterMenuViewModel.OnItemSelected;
		}
	}
}
