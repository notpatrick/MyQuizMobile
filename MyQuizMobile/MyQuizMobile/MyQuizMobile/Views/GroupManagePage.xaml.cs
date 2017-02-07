using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class GroupManagePage : ContentPage {
        public GroupManageViewModel GroupManageViewModel;

        public GroupManagePage() {
            InitializeComponent();
            GroupManageViewModel = new GroupManageViewModel();
            BindingContext = GroupManageViewModel;
        }

        protected override void OnAppearing() {
            MessagingCenter.Unsubscribe<GroupManageViewModel>(this, "Selected");
            MessagingCenter.Subscribe<GroupManageViewModel>(this, "Selected", sender => { listView.SelectedItem = null; });
            base.OnAppearing();
        }

        protected override void OnDisappearing() {
            MessagingCenter.Unsubscribe<GroupManageViewModel>(this, "Selected");
            base.OnDisappearing();
        }
    }
}