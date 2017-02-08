using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class QuestionManagePage : ContentPage {
        public QuestionManageViewModel QuestionManageViewModel;

        public QuestionManagePage() {
            InitializeComponent();
            QuestionManageViewModel = new QuestionManageViewModel();
            BindingContext = QuestionManageViewModel;

            listView.ItemSelected += (s, e) => { listView.SelectedItem = null; };
        }
    }
}