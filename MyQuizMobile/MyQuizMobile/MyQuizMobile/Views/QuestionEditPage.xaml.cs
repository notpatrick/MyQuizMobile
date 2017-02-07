using MyQuizMobile.DataModel;
using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class QuestionEditPage : ContentPage {
        public QuestionEditViewModel QuestionEditViewModel { get; set; }

        public QuestionEditPage(Question q) {
            InitializeComponent();
            QuestionEditViewModel = new QuestionEditViewModel(q);
            BindingContext = QuestionEditViewModel;
        }

        protected override void OnAppearing() {
            MessagingCenter.Unsubscribe<QuestionEditViewModel>(this, "Selected");
            MessagingCenter.Subscribe<QuestionEditViewModel>(this, "Selected", sender => { listView.SelectedItem = null; });
            base.OnAppearing();
        }

        protected override void OnDisappearing() {
            MessagingCenter.Unsubscribe<QuestionEditViewModel>(this, "Selected");
            base.OnDisappearing();
        }
    }
}