using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MyQuizMobile.DataModel;
using MYQuizMobile;
using Xamarin.Forms;

namespace MyQuizMobile {
    public class VeranstaltungenVerwaltenViewModel : INotifyPropertyChanged {
        private readonly Networking _networking;

        private List<Group> _allGroups = new List<Group>();
        private bool _isLoading;

        private string _searchString;
        public bool IsLoading {
            get { return _isLoading; }
            set {
                if (_isLoading != value) {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SearchString {
            get { return _searchString; }
            set {
                if (_searchString != value) {
                    _searchString = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<Group> Groups { get; set; }

        public VeranstaltungenVerwaltenViewModel() {
            Groups = new ObservableCollection<Group>();
            SearchString = string.Empty;
            _networking = App.Networking;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task GetAllGroups() {
            IsLoading = true;
            _allGroups = await _networking.Get<List<Group>>("api/groups/");
            Groups.Clear();
            foreach (var g in _allGroups) {
                Groups.Add(g);
            }
            Filter();
            IsLoading = false;
        }

        public async void addButton_Clicked(object sender, EventArgs e) {
            var nextPage = new VeranstaltungBearbeiten(new Group());
            nextPage.VeranstaltungBearbeitenViewModel.BearbeitenDone += BearbeitenDone;
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
        }

        private async void BearbeitenDone(object sender, MenuItemPickedEventArgs e) {
            var previousPage = await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
            ((VeranstaltungBearbeiten)previousPage).VeranstaltungBearbeitenViewModel.BearbeitenDone -= BearbeitenDone;
            await GetAllGroups();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void listView_Refreshing(object sender, EventArgs e) { await GetAllGroups(); }

        public void searchBar_TextChanged(object sender, TextChangedEventArgs e) { Filter(); }

        public void Filter() {
            IEnumerable<Group> filtered;
            if (SearchString == string.Empty) {
                filtered = _allGroups;
                Groups.Clear();
                foreach (var g in filtered) {
                    Groups.Add(g);
                }
                return;
            }
            filtered = _allGroups.Where(x => x.DisplayText.ToLower().Contains(SearchString.ToLower()));
            Groups.Clear();
            foreach (var g in filtered) {
                Groups.Add(g);
            }
        }

        public async void OnMenuItemTapped(object sender, SelectedItemChangedEventArgs e) {
            var item = e.SelectedItem as Group;

            if (item == null) {
                return;
            }
            var nextPage = new VeranstaltungBearbeiten(item);
            nextPage.VeranstaltungBearbeitenViewModel.BearbeitenDone += BearbeitenDone;
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushAsync(nextPage, true);
            ((ListView)sender).SelectedItem = null;
        }

        public async void OnAppearing(object sender, EventArgs e) { await GetAllGroups(); }
    }
}