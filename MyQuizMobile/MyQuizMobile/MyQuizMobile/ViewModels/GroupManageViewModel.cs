using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class GroupManageViewModel {
        private List<Group> _allGroups = new List<Group>();
        private bool _isSearching;
        private string _searchString = string.Empty;
        private Item _selectedItem;
        public bool IsLoading { get; private set; }
        public ObservableCollection<Group> Groups { get; set; } = new ObservableCollection<Group>();
        public string SearchString {
            get { return _searchString; }
            set {
                _searchString = value;
                SearchCommand.Execute(null);
            }
        }
        public Item SelectedItem {
            get { return _selectedItem; }
            set {
                _selectedItem = value;

                if (_selectedItem == null) {
                    return;
                }
                ItemSelectedCommand.Execute(_selectedItem);
                SelectedItem = null;
            }
        }

        public ICommand AddCommand { get; private set; }
        public ICommand ItemSelectedCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }

        public GroupManageViewModel() { Init(); }

        private async void Init() {
            SubscribeEvents();
            RegisterCommands();
            await GetAllGroups();
        }

        private void SubscribeEvents() {
            MessagingCenter.Unsubscribe<GroupEditViewModel>(this, "Done");
            MessagingCenter.Subscribe<GroupEditViewModel, Group>(this, "Done",
                                                                 async (sender, arg) => { await Finished(); });
            MessagingCenter.Unsubscribe<GroupEditViewModel>(this, "Canceled");
            MessagingCenter.Subscribe<GroupEditViewModel>(this, "Canceled",
                                                          async sender => {
                                                              await ((MasterDetailPage)Application.Current.MainPage)
                                                                  .Detail.Navigation.PopModalAsync(true);
                                                          });
        }

        private void RegisterCommands() {
            AddCommand = new Command(Add);
            ItemSelectedCommand = new Command<Item>(async item => { await ItemSelected(item); });
            RefreshCommand = new Command(async () => { await GetAllGroups(); });
            SearchCommand = new Command(Filter, () => !_isSearching && !IsLoading);
        }

        private async Task GetAllGroups() {
            if (IsLoading) {
                return;
            }
            IsLoading = true;
            ((Command)RefreshCommand).ChangeCanExecute();
            await Task.Run(async () => { _allGroups = await Group.GetAll(); });
            IsLoading = false;
            ((Command)RefreshCommand).ChangeCanExecute();
            SearchCommand.Execute(null);
        }

        private async void Add() {
            var nextPage = new GroupEditPage(new Group());
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushModalAsync(
                                                                                                    new NavigationPage(
                                                                                                                       nextPage),
                                                                                                    true);
        }

        private async Task Finished() {
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopModalAsync(true);
            RefreshCommand.Execute(null);
        }

        private void Filter() {
            _isSearching = true;
            ((Command)SearchCommand).ChangeCanExecute();
            var filtered = string.IsNullOrWhiteSpace(SearchString)
                               ? _allGroups
                               : _allGroups.Where(x => x.DisplayText.ToLower().Contains(SearchString.ToLower()));
            Groups.Clear();
            foreach (var g in filtered) {
                Groups.Add(g);
            }
            _isSearching = false;
            ((Command)SearchCommand).ChangeCanExecute();
        }

        private async Task ItemSelected(Item item) {
            var nextPage = new GroupEditPage((Group)item);
            MessagingCenter.Send(this, "Selected");
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PushModalAsync(
                                                                                                    new NavigationPage(
                                                                                                                       nextPage),
                                                                                                    true);
        }
    }
}