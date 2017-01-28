using MyQuizMobile.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyQuizMobile.ViewModel
{
    public class MyItemsDetailViewModel : ViewModelBase
    {
        public Action<MyItem> OnFinished { get; set; }
        public MyItem Item { get; set; }
        public MyItemsDetailViewModel(MyItem item = null)
        {
            Title = "Edit Item";
            Text = item.Text;
            Description = item.Description;
            Quantity = item.Quantity;

            Item = item;

            SaveCommand = new Command(async () => await ExecuteSaveCommand());
        }
        /// <summary>
        /// Private backing field to hold the text
        /// </summary>
        string text = string.Empty;
        /// <summary>
        /// Public property to set and get the text of the item
        /// </summary>
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        /// <summary>
        /// Private backing field to hold the description
        /// </summary>
        string description = string.Empty;
        /// <summary>
        /// Public property to set and get the description of the item
        /// </summary>
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged(); }
        }

        public Command SaveCommand { get; }

        async Task ExecuteSaveCommand()
        {
            if (IsBusy)
                return;

            Item.Text = Text;
            Item.Description = Description;
            Item.Quantity = Quantity;

            IsBusy = true;

            await StoreManager.MyItemStore.UpdateAsync(Item);
            MyItemsViewModel.IsDirty = true;

            IsBusy = false;
            OnFinished?.Invoke(Item);
        }
    }
}
