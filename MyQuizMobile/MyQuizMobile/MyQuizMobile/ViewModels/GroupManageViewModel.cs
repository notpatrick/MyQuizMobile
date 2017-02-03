using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MyQuizMobile.DataModel;
using MYQuizMobile;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class GroupManageViewModel {
        private readonly Networking _networking;
        private List<Group> _allGroups = new List<Group>();
        public bool IsLoading { get; set; }
        public string SearchString { get; set; }
        public ObservableCollection<Group> Groups { get; set; }

        public GroupManageViewModel() {
            Groups = new ObservableCollection<Group>();
            SearchString = string.Empty;
            _networking = App.Networking;
        }

        private async Task GetAllGroups() {
            IsLoading = true;
            await Task.Run(async () => {
                _allGroups = await _networking.Get<List<Group>>("api/groups/");
                await Filter();
            });
            IsLoading = false;
        }

        public async void addButton_Clicked(object sender, EventArgs e) {
            var nextPage = new GroupEditPage(new Group());
            nextPage.GroupEditViewModel.BearbeitenDone += EditFinished;
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
        }

        private async void EditFinished(object sender, MenuItemPickedEventArgs e) {
            var previousPage = await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
            ((GroupEditPage)previousPage).GroupEditViewModel.BearbeitenDone -= EditFinished;
            await GetAllGroups();
        }

        public async void listView_Refreshing(object sender, EventArgs e) { await GetAllGroups(); }

        public async void searchBar_TextChanged(object sender, TextChangedEventArgs e) { await Filter(); }

        public async Task Filter() {
            await Task.Run(() => {
                var filtered = SearchString == string.Empty
                                   ? _allGroups
                                   : _allGroups.Where(x => x.DisplayText.ToLower().Contains(SearchString.ToLower()));
                Groups.Clear();
                foreach (var g in filtered) {
                    Groups.Add(g);
                }
            });
        }

        public async void OnMenuItemTapped(object sender, SelectedItemChangedEventArgs e) {
            var item = e.SelectedItem as Group;

            if (item == null) {
                return;
            }
            var nextPage = new GroupEditPage(item);
            nextPage.GroupEditViewModel.BearbeitenDone += EditFinished;
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
            ((ListView)sender).SelectedItem = null;
        }

        public async void OnAppearing(object sender, EventArgs e) { await GetAllGroups(); }
    }
}