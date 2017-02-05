using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MyQuizMobile.Views;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class SideMenuViewModel {
        private SideMenuItem _selectedItem;
        public ObservableCollection<SideMenuItem> SideMenuItems { get; set; }
        public SideMenuItem SelectedItem {
            get { return _selectedItem; }
            set {
                _selectedItem = value;

                if (_selectedItem == null) {
                    return;
                }

                ItemTappedCommand.Execute(_selectedItem);
                SelectedItem = null;
            }
        }

        public ICommand ItemTappedCommand { get; set; }

        public SideMenuViewModel() {
            ItemTappedCommand = new Command<SideMenuItem>(OnItemSelected);
            SideMenuItems = new ObservableCollection<SideMenuItem> {
                new SideMenuItem("Abstimmung starten", "Hier können Abstimmungen gestartet werden",
                                 typeof(VotingStartPage)),
                new SideMenuItem("Veranstaltungen verwalten", "Hier können Veranstaltungen verwaltet werden",
                                 typeof(GroupManagePage)),
                new SideMenuItem("Fragelisten verwalten", "Hier können Fragelisten verwaltet werden",
                                 typeof(QuestionBlockManagePage)),
                new SideMenuItem("Fragen verwalten", "Hier können Fragen verwaltet werden", typeof(QuestionManagePage))
            };
        }

        public void OnItemSelected(SideMenuItem item) {
            ((MasterDetailPage)Application.Current.MainPage).Detail =
                new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows) {
                return; // Don't hide sidemenu on UWP
            }
            MessagingCenter.Send(this, "Selected");
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