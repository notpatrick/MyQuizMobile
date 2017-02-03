using System;
using System.Collections.ObjectModel;
using MyQuizMobile.Views;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class SideMenuViewModel {
        public ObservableCollection<SideMenuItem> SideMenuItems { get; set; }

        public SideMenuViewModel() {
            SideMenuItems = new ObservableCollection<SideMenuItem> {
                new SideMenuItem("Abstimmung starten", "Hier können Abstimmungen gestartet werden",
                                 typeof(VotingStartPage)),
                new SideMenuItem("Veranstaltungen verwalten", "Hier können Veranstaltungen verwaltet werden",
                                 typeof(GroupManagePage)),
                new SideMenuItem("Fragelisten verwalten", "Hier können Fragelisten verwaltet werden",
                                 typeof(QuestionBlockManagePage)),
                new SideMenuItem("Fragen verwalten", "Hier können Fragen verwaltet werden",
                                 typeof(QuestionManagePage))
            };
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e) {
            var item = e.SelectedItem as SideMenuItem;
            if (item == null) {
                return;
            }
            ((MasterDetailPage)Application.Current.MainPage).Detail =
                new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows) {
                return;
            }
            ((ListView)sender).SelectedItem = null;
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