using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class GroupManagePage : ContentPage {
        public GroupManageViewModel GroupManageViewModel;

        public GroupManagePage() {
            InitializeComponent();
            GroupManageViewModel = new GroupManageViewModel();
            BindingContext = GroupManageViewModel;
            listView.ItemSelected += (s, e) => { listView.SelectedItem = null; };
        }
    }
}