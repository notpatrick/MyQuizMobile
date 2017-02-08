using MyQuizMobile.DataModel;
using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class QuestionEditPage : ContentPage {
        public QuestionEditViewModel QuestionEditViewModel { get; set; }

        public QuestionEditPage(Question q) {
            InitializeComponent();
            QuestionEditViewModel = new QuestionEditViewModel(q);
            BindingContext = QuestionEditViewModel;
            listView.ItemSelected += (s, e) => { listView.SelectedItem = null; };
        }
    }
}