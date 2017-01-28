using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyQuizMobile.View
{
	public partial class RootMasterDetailPage : MasterDetailPage
	{

        Dictionary<int, Page> StoredPages;
		public RootMasterDetailPage ()
		{
			InitializeComponent ();
            StoredPages = new Dictionary<int, Page>();
            RootMasterPage.ListView.ItemSelected += ListView_ItemSelected;

            StoredPages.Add(0, Detail);

            this.MasterBehavior = MasterBehavior.Popover;
        }



        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterMenuItem;
            if (item == null)
                return;

            //Allow navigation drawer to close propertly
            IsPresented = false;
            await Task.Delay(230);

            if (StoredPages.ContainsKey(item.Id))
            {
                Detail = StoredPages[item.Id];
            }
            else
            {

                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;
                Page newPage = null;
                Detail = newPage = new NavigationPage(page)
                {
                    BarBackgroundColor = (Color)Application.Current.Resources["Primary"],
                    BarTextColor = Color.White
                };
                StoredPages.Add(item.Id, newPage);
            }

            RootMasterPage.ListView.SelectedItem = null;
            
        }
    }
}
