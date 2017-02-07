using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class QuestionBlockManagePage : ContentPage {
        public QuestionBlockManageViewModel QuestionBlockManageViewModel;

        public QuestionBlockManagePage() {
            InitializeComponent();
            QuestionBlockManageViewModel = new QuestionBlockManageViewModel();
            BindingContext = QuestionBlockManageViewModel;
        }

        protected override void OnAppearing() {
            MessagingCenter.Unsubscribe<QuestionBlockManageViewModel>(this, "Selected");
            MessagingCenter.Subscribe<QuestionBlockManageViewModel>(this, "Selected", sender => { listView.SelectedItem = null; });
            base.OnAppearing();
        }

        protected override void OnDisappearing() {
            MessagingCenter.Unsubscribe<QuestionBlockManageViewModel>(this, "Selected");
            base.OnDisappearing();
        }
    }
}