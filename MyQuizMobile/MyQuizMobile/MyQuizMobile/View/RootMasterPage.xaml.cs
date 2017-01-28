using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyQuizMobile.View
{
	public partial class RootMasterPage : ContentPage
	{
        public ListView ListView => ListViewMenuItems;


        public RootMasterPage ()
		{
			InitializeComponent ();
            BindingContext = new MasterViewModel();
        }
	}

    class MasterMenuItem
    {
        public MasterMenuItem()
        {
           
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }

    class MasterViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MasterMenuItem> MenuItems { get; }
        public MasterViewModel()
        {
            MenuItems = new ObservableCollection<MasterMenuItem>(new[]
            {
                    new MasterMenuItem { Id = 0, Title = "Browse", TargetType = typeof(BrowseItemsPage) },
                    new MasterMenuItem { Id = 1, Title = "My Items", TargetType = typeof(MyItemsPage) },
                    new MasterMenuItem { Id = 2, Title = "About", TargetType = typeof(AboutPage) },
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
