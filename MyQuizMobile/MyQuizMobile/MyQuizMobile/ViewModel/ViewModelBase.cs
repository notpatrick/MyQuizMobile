using MyQuizMobile.Helpers;
using MyQuizMobile.Interfaces;
using MyQuizMobile.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyQuizMobile.ViewModel
{
    public class ViewModelBase : ObservableObject
    {
        /// <summary>
        /// Get the azure service instance
        /// </summary>
        public IStoreManager StoreManager => DependencyService.Get<IStoreManager>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
    }
}
