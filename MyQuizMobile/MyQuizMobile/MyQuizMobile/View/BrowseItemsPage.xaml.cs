using MyQuizMobile.Model;
using MyQuizMobile.ViewModel;

using Xamarin.Forms;

namespace MyQuizMobile.View
{
    public partial class BrowseItemsPage : ContentPage
    {
        BrowseItemsViewModel viewModel;
        public BrowseItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new BrowseItemsViewModel();

            viewModel.OnNavigateToDetails = async (detailsViewModel) =>
            {
                await Navigation.PushAsync(new DetailPage(detailsViewModel));
            };
            
        }

        void OnItemSelected (object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            viewModel.GoToDetailsCommand.Execute(item.Id);

            // Manually deselect item
            ListViewItems.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

    }
}
