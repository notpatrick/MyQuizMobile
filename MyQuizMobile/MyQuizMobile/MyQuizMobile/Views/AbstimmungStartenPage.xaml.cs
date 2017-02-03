using Xamarin.Forms;

namespace MyQuizMobile {
    public partial class AbstimmungStartenPage : ContentPage {
        public AbstimmungStartenViewModel AbstimmungStartenViewModel;

        public AbstimmungStartenPage() {
            InitializeComponent();
            AbstimmungStartenViewModel = new AbstimmungStartenViewModel();
            BindingContext = AbstimmungStartenViewModel;

            listView.ItemSelected += AbstimmungStartenViewModel.OnMenuItemTapped;
            timeEntry.Focused += AbstimmungStartenViewModel.timeEntry_OnFocused;
            timeEntry.Unfocused += AbstimmungStartenViewModel.timeEntry_OnUnfocused;
            weiterButton.Clicked += AbstimmungStartenViewModel.weiterButton_Clicked;
            personenbezogenSwitch.Toggled += AbstimmungStartenViewModel.personenbezogenSwitch_Toggled;
        }

        protected override void OnDisappearing() {
            listView.SelectedItem = null;
            base.OnDisappearing();
        }
    }
}