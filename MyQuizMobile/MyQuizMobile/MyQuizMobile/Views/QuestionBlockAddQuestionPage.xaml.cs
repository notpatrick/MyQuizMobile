using MyQuizMobile.DataModel;
using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class QuestionBlockAddQuestionPage : ContentPage {
        public QuestionBlockAddQuestionViewModel QuestionBlockAddQuestionViewModel;

        public QuestionBlockAddQuestionPage(QuestionBlock qb) {
            InitializeComponent();
            QuestionBlockAddQuestionViewModel = new QuestionBlockAddQuestionViewModel(qb);
            BindingContext = QuestionBlockAddQuestionViewModel;
            NavigationPage.SetHasBackButton(this, false);
            listView.ItemSelected += (sender, args) => { listView.SelectedItem = null; };
        }

        private void SwitchCell_OnOnChanged(object sender, ToggledEventArgs e) { QuestionBlockAddQuestionViewModel.Switched(sender, e); }
    }
}