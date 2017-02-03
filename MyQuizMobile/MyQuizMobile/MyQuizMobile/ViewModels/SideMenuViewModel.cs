using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using PostSharp.Patterns.Model;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class SideMenuViewModel {
        public ObservableCollection<SideMenuItem> SideMenuItems { get; set; }

        public SideMenuViewModel() {
            SideMenuItems = new ObservableCollection<SideMenuItem> {
                new SideMenuItem("Abstimmung starten", "Hier können Abstimmungen gestartet werden",
                                 typeof(AbstimmungStartenPage)),
                new SideMenuItem("Veranstaltungen verwalten", "Hier können Veranstaltungen verwaltet werden",
                                 typeof(VeranstaltungenVerwaltenPage)),
                new SideMenuItem("Fragelisten verwalten", "Hier können Fragelisten verwaltet werden",
                                 typeof(VeranstaltungenVerwaltenPage)),
                new SideMenuItem("Fragen verwalten", "Hier können Fragen verwaltet werden",
                                 typeof(VeranstaltungenVerwaltenPage))
            };
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e) {
            var item = e.SelectedItem as SideMenuItem;

            if (item == null) {
                return;
            }
            ((MasterDetailPage)Application.Current.MainPage).Detail =
                new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

            ((ListView)sender).SelectedItem = null;
            if(Device.OS != TargetPlatform.WinPhone && Device.OS != TargetPlatform.Windows)
                ((MasterDetailPage)Application.Current.MainPage).IsPresented = false;
        }
    }

    public class SideMenuItem {
        public string Title { get; set; }
        public string Detail { get; set; }
        public Type TargetType { get; set; }

        public SideMenuItem(string title, string detail, Type target) {
            Title = title;
            Detail = detail;
            TargetType = target;
        }
    }
}