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
            listView.ItemSelected += (s, e) => { listView.SelectedItem = null; };
        }
    }
}