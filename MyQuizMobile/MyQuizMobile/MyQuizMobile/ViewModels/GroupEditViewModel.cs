using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyQuizMobile.DataModel;
using MYQuizMobile;
using PostSharp.Patterns.Model;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class GroupEditViewModel {
        private readonly Networking _networking;
        private List<SingleTopic> _singleTopics = new List<SingleTopic>();
        public Group Group { get; set; }

        public GroupEditViewModel(Group group) {
            Group = group;
            _networking = App.Networking;
        }

        private async Task GetAllSingleTopics() {
            await Task.Run(async () => {
                _singleTopics = await _networking.Get<List<SingleTopic>>($"api/groups/{Group.Id}/topics");
                Group.topicList.Clear();
                foreach (var g in _singleTopics) {
                    Group.topicList.Add(g);
                }
            });
        }

        public void cancelButton_Clicked(object sender, EventArgs e) {
            OnDone(new MenuItemPickedEventArgs {Item = Group});
        }

        public async void saveButton_Clicked(object sender, EventArgs e) {
            var res = await _networking.Post("api/groups/", Group);
            await _networking.Post($"api/groups/{res}/topics/", Group.topicList);
            OnDone(new MenuItemPickedEventArgs {Item = Group});
        }

        public async void deleteButton_Clicked(object sender, EventArgs e) {
            await _networking.Delete($"api/groups/{Group.Id}");
            OnDone(new MenuItemPickedEventArgs {Item = Group});
        }

        public async void OnAppearing(object sender, EventArgs e) { await GetAllSingleTopics(); }

        #region event
        public event BearbeitenDoneHanler BearbeitenDone;

        public delegate void BearbeitenDoneHanler(object sender, MenuItemPickedEventArgs e);

        protected virtual void OnDone(MenuItemPickedEventArgs e) { BearbeitenDone?.Invoke(this, e); }
        #endregion
    }
}