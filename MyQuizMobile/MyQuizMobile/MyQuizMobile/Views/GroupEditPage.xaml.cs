using MyQuizMobile.DataModel;
using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class GroupEditPage : ContentPage {
        public GroupEditViewModel GroupEditViewModel;

        public GroupEditPage(Group group) {
            InitializeComponent();
            GroupEditViewModel = new GroupEditViewModel(group);
            BindingContext = GroupEditViewModel;
            cancelButton.Clicked += GroupEditViewModel.cancelButton_Clicked;
            saveButton.Clicked += GroupEditViewModel.saveButton_Clicked;
            deleteButton.Clicked += GroupEditViewModel.deleteButton_Clicked;
            Appearing += GroupEditViewModel.OnAppearing;
        }
    }
}