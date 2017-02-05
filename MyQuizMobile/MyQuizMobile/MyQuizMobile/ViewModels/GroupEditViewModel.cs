using System.Linq;
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
            CanDelete = Group.topicList.Any();
            DeleteCommand = new Command(Delete);
            SaveCommand = new Command(Save);
            CancelCommand = new Command(Cancel);
            RemoveSingleTopicCommand = new Command<SingleTopic>(RemoveSingleTopic);
            AddSingleTopicCommand = new Command(Add);
        }

        private void Cancel() { MessagingCenter.Send(this, "Canceled"); }

        private async void Save() {
            Group.topicList.Remove(x => string.IsNullOrWhiteSpace(x.Name));
            await Group.Post(Group);
            MessagingCenter.Send(this, "Done", Group);
        }

        private async void Delete() {
            await Group.DeleteById(Group.Id);
            MessagingCenter.Send(this, "Done", Group);
        }

        private void Add() { Group.topicList.Add(new SingleTopic()); }

        private void RemoveSingleTopic(SingleTopic st) {
            if (Group.topicList.Contains(st)) {
                Group.topicList.Remove(st);
            }
        }
    }
}