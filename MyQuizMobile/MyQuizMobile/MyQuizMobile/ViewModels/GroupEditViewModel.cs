using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;
using Xamarin.Forms;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class GroupEditViewModel {
        public Group Group { get; set; }
        public bool CanDelete { get; set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand RemoveSingleTopicCommand { get; set; }
        public ICommand AddSingleTopicCommand { get; set; }

        public GroupEditViewModel(Group group) {
            Group = group;
            CanDelete = Group.SingleTopics.Any();
            DeleteCommand = new Command(async () => { await Delete(); });
            SaveCommand = new Command(async () => { await Save(); });
            CancelCommand = new Command(async () => { await Cancel(); });
            RemoveSingleTopicCommand = new Command<SingleTopic>(RemoveSingleTopic);
            AddSingleTopicCommand = new Command(Add);
        }

        private async Task Cancel() { await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true); }

        private async Task Save() {
            Group.SingleTopics.Remove(x => string.IsNullOrWhiteSpace(x.Name));
            await Group.Post(Group);
            MessagingCenter.Send(this, "Done", Group);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
        }

        private async Task Delete() {
            await Group.DeleteById(Group.Id);
            MessagingCenter.Send(this, "Done", Group);
            await ((MasterDetailPage)Application.Current.MainPage).Detail.Navigation.PopAsync(true);
        }

        private void Add() { Group.SingleTopics.Add(new SingleTopic()); }

        private void RemoveSingleTopic(SingleTopic st) {
            if (Group.SingleTopics.Contains(st)) {
                Group.SingleTopics.Remove(st);
            }
        }
    }
}