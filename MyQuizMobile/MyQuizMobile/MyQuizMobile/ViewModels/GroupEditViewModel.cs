using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class GroupEditViewModel {
        public Group Group { get; set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public GroupEditViewModel(Group group) {
            Group = group;
            DeleteCommand = new Command(Delete);
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
        }

        private void Cancel() { OnDone(new MenuItemPickedEventArgs {Item = Group}); }

        private async void Save() {
            await Group.Post(Group);
            OnDone(new MenuItemPickedEventArgs {Item = Group});
        }

        private async void Delete() {
            await Group.DeleteById(Group.Id);
            OnDone(new MenuItemPickedEventArgs {Item = Group});
        }

        #region event
        public event BearbeitenDoneHanler BearbeitenDone;

        public delegate void BearbeitenDoneHanler(object sender, MenuItemPickedEventArgs e);

        protected virtual void OnDone(MenuItemPickedEventArgs e) { BearbeitenDone?.Invoke(this, e); }
        #endregion
    }
}