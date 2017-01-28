using MyQuizMobile.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyQuizMobile.Services.Standard
{
    public class BaseStore<T> : IBaseStore<T> where T : class, IBaseDataObject, new()
    {
        public virtual string Identifier => "Items";

        List<T> table;
       
        public void DropTable()
        {
            table = new List<T>();
        }

        public BaseStore()
        {
            table = new List<T>();
        }

        #region IBaseStore implementation

        public Task InitializeStore()
        {
            return Task.FromResult(true);
        }

        public virtual async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(table);
        }

        public virtual Task<T> GetItemAsync(string id)
        {
            return Task.FromResult(table.FirstOrDefault(s => s.Id == id));
        }

        public virtual Task<bool> InsertAsync(T item)
        {
            table.Add(item);
            return Task.FromResult(true);
        }

        public virtual async Task<bool> UpdateAsync(T item)
        {
            var realItem = await GetItemAsync(item.Id);
            if (realItem == null)
                return false;


            var index = table.IndexOf(realItem);

            if (index < 0)
                return false;

            table[index] = item;
            return true;
        }

        public virtual Task<bool> RemoveAsync(T item)
        {
            table.Remove(item);
            return Task.FromResult(true);
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }


        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        #endregion
    }
}