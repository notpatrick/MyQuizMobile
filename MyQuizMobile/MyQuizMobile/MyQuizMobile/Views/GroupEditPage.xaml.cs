using MyQuizMobile.DataModel;
using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class GroupEditPage : ContentPage {
        public GroupEditViewModel GroupEditViewModel;

        public GroupEditPage(Group group) {
            InitializeComponent();
            GroupEditViewModel = new GroupEditViewModel(group);
            BindingContext = GroupEditViewModel;
        }
    }
}