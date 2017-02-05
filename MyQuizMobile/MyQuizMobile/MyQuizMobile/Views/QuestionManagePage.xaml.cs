using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class QuestionManagePage : ContentPage {
        public QuestionManageViewModel QuestionManageViewModel;

        public QuestionManagePage() {
            InitializeComponent();
            QuestionManageViewModel = new QuestionManageViewModel();
            BindingContext = QuestionManageViewModel;
        }

        protected override void OnAppearing() {
            MessagingCenter.Unsubscribe<QuestionManageViewModel>(this, "Selected");
            MessagingCenter.Subscribe<QuestionManageViewModel>(this, "Selected",
                                                               sender => { listView.SelectedItem = null; });
            base.OnAppearing();
        }

        protected override void OnDisappearing() {
            MessagingCenter.Unsubscribe<QuestionManageViewModel>(this, "Selected");
            base.OnDisappearing();
        }
    }
}