using MyQuizMobile.Helpers;
using MyQuizMobile.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyQuizMobile.ViewModel
{
    public class BrowseItemsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Item> Items { get;}
        public Action<ItemDetailViewModel> OnNavigateToDetails { get; set; }
        public BrowseItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableRangeCollection<Item>();
            LoadItemsCommand = new Command(ExecuteLoadItemsCommand);
            GoToDetailsCommand = new Command<string>(ExecuteGoToDetailsCommand);
        }

        public Command LoadItemsCommand { get;}

        async void ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Items.Clear();
                var items = await StoreManager.ItemStore.GetItemsAsync(true);
                Items.ReplaceRange(items);
            }
            catch (Exception ex)
            {
                //Handle exception here
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
       

        public Command<string> GoToDetailsCommand { get; }
        ItemDetailViewModel detailsViewModel;
        void ExecuteGoToDetailsCommand(string id)
        {
            if (IsBusy)
                return;

            var selectedItem = Items.FirstOrDefault(i => i.Id == id);

            detailsViewModel = new ItemDetailViewModel(selectedItem);
            detailsViewModel.OnFinished += OnFinished;

            OnNavigateToDetails(detailsViewModel);
        }

        void OnFinished(Item item)
        {
            detailsViewModel.OnFinished -= OnFinished;
        }

    }
}
