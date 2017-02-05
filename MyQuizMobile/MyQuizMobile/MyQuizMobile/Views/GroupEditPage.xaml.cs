using MyQuizMobile.DataModel;
using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class GroupEditPage : ContentPage {
        public GroupEditViewModel GroupEditViewModel;

        public GroupEditPage(Group group) {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            GroupEditViewModel = new GroupEditViewModel(group);
            BindingContext = GroupEditViewModel;
        }

        protected override void OnAppearing() {
            MessagingCenter.Unsubscribe<GroupEditViewModel>(this, "Selected");
            MessagingCenter.Subscribe<GroupEditViewModel>(this, "Selected", sender => { listView.SelectedItem = null; });
            base.OnAppearing();
        }

        protected override void OnDisappearing() {
            MessagingCenter.Unsubscribe<GroupEditViewModel>(this, "Selected");
            base.OnDisappearing();
        }
    }
}