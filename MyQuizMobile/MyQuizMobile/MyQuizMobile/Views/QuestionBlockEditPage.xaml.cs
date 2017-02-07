using MyQuizMobile.DataModel;
using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class QuestionBlockEditPage : ContentPage {
        public QuestionBlockEditViewModel QuestionBlockEditViewModel;

        public QuestionBlockEditPage(QuestionBlock qb) {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            QuestionBlockEditViewModel = new QuestionBlockEditViewModel(qb);
            BindingContext = QuestionBlockEditViewModel;
        }

        protected override void OnAppearing() {
            MessagingCenter.Unsubscribe<QuestionBlockEditViewModel>(this, "Selected");
            MessagingCenter.Subscribe<QuestionBlockEditViewModel>(this, "Selected", sender => { listView.SelectedItem = null; });
            base.OnAppearing();
        }

        protected override void OnDisappearing() {
            MessagingCenter.Unsubscribe<QuestionBlockEditViewModel>(this, "Selected");
            base.OnDisappearing();
        }
    }
}