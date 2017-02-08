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
        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e) { listView.SelectedItem = null; }
    }
}