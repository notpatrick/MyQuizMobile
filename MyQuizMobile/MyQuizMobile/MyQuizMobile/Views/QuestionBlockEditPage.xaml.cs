using MyQuizMobile.DataModel;
using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class QuestionBlockEditPage : ContentPage {
        public QuestionBlockEditViewModel QuestionBlockEditViewModel;

        public QuestionBlockEditPage(QuestionBlock qb) {
            InitializeComponent();
            QuestionBlockEditViewModel = new QuestionBlockEditViewModel(qb);
            BindingContext = QuestionBlockEditViewModel;
            listView.ItemSelected += (s, e) => { listView.SelectedItem = null; };
        }
    }
}