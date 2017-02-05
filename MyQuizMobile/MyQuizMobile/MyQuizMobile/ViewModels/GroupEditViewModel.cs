using System.Windows.Input;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class GroupEditViewModel {
        public Group Group { get; set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand ItemSelectedCommand { get; set; }

        public GroupEditViewModel(Group group) {
            Group = group;
            DeleteCommand = new Command(Delete);
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
            ItemSelectedCommand = new Command(ItemSelected);
        }

        private void Cancel() { MessagingCenter.Send(this, "Canceled"); }

        private async void Save() {
            await Group.Post(Group);
            MessagingCenter.Send(this, "Done", Group);
        }

        private async void Delete() {
            await Group.DeleteById(Group.Id);
            MessagingCenter.Send(this, "Done", Group);
        }

        private void ItemSelected() {
            // todo: singletopic changes...
        }
    }
}