using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MyQuizMobile
{
	public partial class RootView : MasterDetailPage
	{
	    public RootViewViewModel RootViewViewModel;
	    public static NavigationPage rootNavigationPage;

	    public static RootView rootView
	    {
	        get;
	        set;
	    }

	    public RootView()
		{
			InitializeComponent();
		    rootView = this;
            RootViewViewModel = DependencyService.Get<RootViewViewModel>();
            BindingContext = RootViewViewModel;
            rootNavigationPage = rootNavigation;
		}
	}
}
