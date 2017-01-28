using System;
using Xamarin.Forms;
using MyQuizMobile;

[assembly: Xamarin.Forms.Dependency(typeof(DetailMainViewModel))]
namespace MyQuizMobile
{
    public class DetailMainViewModel
    {
        private readonly DetailMain _detailMain;

        public DetailMainViewModel(DetailMain detailMain)
        {
            _detailMain = detailMain;
        }

        // TODO: refactor this to not depend on placeholder text of entry
        public async void OnVeranstaltungTap(object sender, EventArgs eventArgs)
        {
            var entry = (sender as Entry);
            if (entry == null) return;
            entry.Unfocus();
            var name = entry.Placeholder;
            switch (name)
            {
                case "Veranstaltungen":
                    await RootView.rootNavigationPage.Navigation.PushAsync(new EntrySelectPage(name, _detailMain));
                    entry.Unfocus();
                    break;
                default:
                    break;
            }
        }
    }
}
