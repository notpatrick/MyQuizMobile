using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace MyQuizMobile {
    public static class Extensions {
        public static int Remove<T>(this ObservableCollection<T> coll, Func<T, bool> condition) {
            var itemsToRemove = coll.Where(condition).ToList();

            foreach (var itemToRemove in itemsToRemove) {
                coll.Remove(itemToRemove);
            }

            return itemsToRemove.Count;
        }

        public static void Invalidate(this View view)
        {
            if (view == null)
            {
                return;
            }

            var method = typeof(View).GetMethod("InvalidateMeasure", BindingFlags.Instance | BindingFlags.NonPublic);

            method.Invoke(view, null);
        }
    }


}