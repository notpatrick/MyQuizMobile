using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MyQuizMobile.Views;
using Xamarin.Forms;

namespace MyQuizMobile
{
    public class SideMenuViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SideMenuItem> SideMenuItems { get; set; }

        public SideMenuViewModel()
        {
            SideMenuItems = new ObservableCollection<SideMenuItem>
            {
                new SideMenuItem("Abstimmung starten", "Hier können Abstimmungen gestartet werden",typeof(AbstimmungStartenPage)),
                new SideMenuItem("Veranstaltungen verwalten", "Hier können Veranstaltungen verwaltet werden",typeof(VeranstaltungenVerwaltenPage)),
                new SideMenuItem("Fragelisten verwalten", "Hier können Fragelisten verwaltet werden",typeof(VeranstaltungenVerwaltenPage)),
                new SideMenuItem("Fragen verwalten", "Hier können Fragen verwaltet werden",typeof(VeranstaltungenVerwaltenPage))
            };
        }

        public async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as SideMenuItem;

            if (item == null) return;
            ((MasterDetailPage)Application.Current.MainPage).Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

            if (Device.Idiom == TargetIdiom.Desktop) return;
            await Task.Delay(150);
            (sender as ListView).SelectedItem = null;
            ((MasterDetailPage)Application.Current.MainPage).IsPresented = false;
        }

        #region propertychanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion  
    }

    public class SideMenuItem
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public Type TargetType { get; set; }

        public SideMenuItem(string title, string detail, Type target)
        {
            Title = title;
            Detail = detail;
            TargetType = target;
        }
    }
}
