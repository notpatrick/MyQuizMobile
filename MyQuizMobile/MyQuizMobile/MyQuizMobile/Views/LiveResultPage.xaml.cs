using Xamarin.Forms;

namespace MyQuizMobile
{
    public partial class LiveResultPage : ContentPage
    {
        public LiveResultViewModel LiveResultViewModel;

        public LiveResultPage(AbstimmungStartenViewModel asvm)
        {
            InitializeComponent();
            LiveResultViewModel = new LiveResultViewModel(asvm);
            BindingContext = LiveResultViewModel;

            weiterButton.Clicked += LiveResultViewModel.weiterButton_Clicked;
            timeEntry.Focused += LiveResultViewModel.timeEntry_OnFocused;
            timeEntry.Unfocused += LiveResultViewModel.timeEntry_OnUnfocused;
            listView.ItemTapped += LiveResultViewModel.listView_ItemTapped;
            personPicker.ItemSelected += LiveResultViewModel.personPicker_ItemSelected;
        }
        
        protected override bool OnBackButtonPressed()
        {
            return LiveResultViewModel.OnBackButtonPressed(this);
        }
    }
}