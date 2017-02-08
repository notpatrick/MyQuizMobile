using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class QuestionBlockManagePage : ContentPage {
        public QuestionBlockManageViewModel QuestionBlockManageViewModel;

        public QuestionBlockManagePage() {
            InitializeComponent();
            QuestionBlockManageViewModel = new QuestionBlockManageViewModel();
            BindingContext = QuestionBlockManageViewModel;
            listView.ItemSelected += (s, e) => { listView.SelectedItem = null; };
        }
    }
}