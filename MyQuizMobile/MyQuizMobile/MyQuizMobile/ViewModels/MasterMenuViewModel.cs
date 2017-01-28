using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using MyQuizMobile;

[assembly: Xamarin.Forms.Dependency(typeof(MasterMenuViewModel))]
namespace MyQuizMobile
{
    public class MasterMenuViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MasterMenuItems> MasterMenuItems { get; set; }

        public MasterMenuViewModel()
        {
            MasterMenuItems = new ObservableCollection<MasterMenuItems>
            {
                new MasterMenuItems("Fragen senden", "Hier können Fragen versendet werden",typeof(DetailMain)),
                new MasterMenuItems("Fragen bearbeiten", "Hier können Fragen bearbeitet werden",typeof(DetailTwo)),
                new MasterMenuItems("Veranstaltungen verwalten", "Hier können Veranstaltungen verwaltet werden",typeof(DetailThree)),
                new MasterMenuItems("Ja so gehts", "blabla", typeof(DetailFour)),
                new MasterMenuItems(),
                new MasterMenuItems(),
                new MasterMenuItems(),
                new MasterMenuItems(),
            };
        }

        public async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterMenuItems;

            if (item == null) return;
            RootView.rootView.Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

            if (Device.Idiom == TargetIdiom.Desktop) return;
            await Task.Delay(150);
            (sender as ListView).SelectedItem = null;
            RootView.rootView.IsPresented = false;
        }

        #region propertychanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion  
    }

    public class MasterMenuItems
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public Type TargetType { get; set; }
        public MasterMenuItems()
        {
            Title = "Default Title";
            Detail = "Default Detail";
            TargetType = typeof(DetailMain);
        }

        public MasterMenuItems(string title, string detail, Type target)
        {
            Title = title;
            Detail = detail;
            TargetType = target;
        }
    }
}
