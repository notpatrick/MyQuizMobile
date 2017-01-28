using System.Collections.Generic;
using System.Threading.Tasks;
using MyQuizMobile.Interfaces;
using MyQuizMobile.Model;
using MyQuizMobile.Services;
using Xamarin.Forms;
using System.Linq;
using MyQuizMobile.Services.Standard;

[assembly: Dependency(typeof(ItemStore))]
namespace MyQuizMobile.Services.Standard
{
    public class ItemStore : BaseStore<Item>, IItemStore
    {
        public override string Identifier => "Item";

        public override async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            var items = await base.GetItemsAsync(forceRefresh);
            if (!items.Any())
            {
                //If no items have been entered and azure needs setup then we can add a bit of temp data
                var newItem = new Item
                {
                    Text = "Temp Item",
                    Description = "This is an item that was created for offline demo purposes"
                };
                await base.InsertAsync(newItem);
                return new[] { newItem };
            }
            return items;
        }
    }
}
