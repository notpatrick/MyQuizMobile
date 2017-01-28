using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyQuizMobile
{
    
    public partial class EntrySelectPage : ContentPage
    {
        public EntrySelectPageViewModel EntrySelectPageViewModel;

        public EntrySelectPage(string type, DetailMain detailMain)
        {
			InitializeComponent ();
            EntrySelectPageViewModel = new EntrySelectPageViewModel(type, detailMain);
            BindingContext = EntrySelectPageViewModel;
            listAuswahl.ItemSelected += EntrySelectPageViewModel.OnItemSelected;
        }
    }
}
