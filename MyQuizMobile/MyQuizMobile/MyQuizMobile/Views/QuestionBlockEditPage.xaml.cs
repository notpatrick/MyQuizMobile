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
    }
}