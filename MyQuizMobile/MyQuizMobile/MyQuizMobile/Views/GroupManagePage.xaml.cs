using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class GroupManagePage : ContentPage {
        public GroupManageViewModel GroupManageViewModel;

        public GroupManagePage() {
            InitializeComponent();
            GroupManageViewModel = new GroupManageViewModel();
            BindingContext = GroupManageViewModel;

            addButton.Clicked += GroupManageViewModel.addButton_Clicked;
            listView.Refreshing += GroupManageViewModel.listView_Refreshing;
            searchBar.TextChanged += GroupManageViewModel.searchBar_TextChanged;
            listView.ItemSelected += GroupManageViewModel.OnMenuItemTapped;
            Appearing += GroupManageViewModel.OnAppearing;
        }
    }
}