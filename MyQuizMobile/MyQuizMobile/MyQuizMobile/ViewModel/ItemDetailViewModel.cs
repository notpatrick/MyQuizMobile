using MyQuizMobile.Helpers;
using MyQuizMobile.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyQuizMobile.ViewModel
{
    public class ItemDetailViewModel : ViewModelBase
    {
        public Action<Item> OnFinished { get; set; }
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item.Text;
            Item = item;
            SaveCommand = new Command(async () => await ExecuteSaveCommand());
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }

        public Command SaveCommand { get; }

        async Task ExecuteSaveCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var newItem = new MyItem
            {
                Text = Item.Text,
                Description = Item.Description,
                Quantity = Quantity
            };

            try
            {
                if (!Settings.IsLoggedIn)
                {
                    if (!await LoginViewModel.TryLoginAsync(StoreManager))
                        return;
                }

                await StoreManager.MyItemStore.InsertAsync(newItem);
                MyItemsViewModel.IsDirty = true;

                IsBusy = false;
                OnFinished?.Invoke(Item);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
