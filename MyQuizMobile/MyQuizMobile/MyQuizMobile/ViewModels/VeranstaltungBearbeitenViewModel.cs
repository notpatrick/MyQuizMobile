using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MyQuizMobile.DataModel;
using MyQuizMobile.Droid.Annotations;
using MYQuizMobile;

namespace MyQuizMobile {
    public class VeranstaltungBearbeitenViewModel : INotifyPropertyChanged {
        private readonly Networking _networking;
        private Group _group;
        public Group Group {
            get { return _group; }
            set {
                if (_group != value) {
                    _group = value;
                    OnPropertyChanged();
                }
            }
        }
        private List<SingleTopic> _allSingleTopics = new List<SingleTopic>();

        public VeranstaltungBearbeitenViewModel(Group group) {
            Group = group;
            _networking = App.Networking;
        }

        private async Task GetAllSingleTopics()
        {
            _allSingleTopics = await _networking.Get<List<SingleTopic>>($"api/groups/{Group.Id}/topics");
            Group.SingleTopics.Clear();
            foreach (var g in _allSingleTopics)
            {
                Group.SingleTopics.Add(g);
            }
        }

        public void abbrechenButton_Clicked(object sender, EventArgs e) {
            OnDone(new MenuItemPickedEventArgs {Item = Group});
        }

        public async void saveButton_Clicked(object sender, EventArgs e) {
            await _networking.Post("api/groups/", Group);
            OnDone(new MenuItemPickedEventArgs {Item = Group});
        }

        public async void löschenButton_Clicked(object sender, EventArgs e) {
            await _networking.Delete($"api/groups/{Group.Id}");
            OnDone(new MenuItemPickedEventArgs {Item = Group});
        }

        #region notify
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region event
        public event BearbeitenDoneHanler BearbeitenDone;

        public delegate void BearbeitenDoneHanler(object sender, MenuItemPickedEventArgs e);

        protected virtual void OnDone(MenuItemPickedEventArgs e) { BearbeitenDone?.Invoke(this, e); }
        #endregion

        public async void OnAppearing(object sender, EventArgs e) { await GetAllSingleTopics(); }
    }
}