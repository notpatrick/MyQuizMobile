using System;
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
            timeEntry.Focused += LiveResultViewModel.TimeEntryOnFocused;
            timeEntry.Unfocused += LiveResultViewModel.TimeEntryOnUnfocused;
            LiveResultViewModel.TimeEntryOnUnfocused(null,new FocusEventArgs(timeEntry, false));
        }
        
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Achtung!",
                    "Wollen Sie die aktuelle Umfrage wirklich vorzeitig beenden? Ergebnisse können dann unvollständig sein!",
                    "Umfrage Beenden", "Zurück");
                if (result)
                    await ((MasterDetailPage) Application.Current.MainPage).Detail.Navigation.PopAsync();
                // or anything else
            });

            return true;
        }
    }
}