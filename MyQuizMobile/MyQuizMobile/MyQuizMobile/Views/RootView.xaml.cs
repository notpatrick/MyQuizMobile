using Xamarin.Forms;

namespace MyQuizMobile
{
	public partial class RootView : MasterDetailPage
	{
	    public RootViewViewModel RootViewViewModel;

	    public RootView()
		{
			InitializeComponent();
            RootViewViewModel = new RootViewViewModel();
            BindingContext = RootViewViewModel;
		}
	}
}
