using System;
using MyQuizMobile.DataModel;
using PostSharp.Patterns.Model;

namespace MyQuizMobile {
    [NotifyPropertyChanged]
    public class GroupEditViewModel {
        public Group Group { get; set; }

        public GroupEditViewModel(Group group) { Group = group; }

        public void cancelButton_Clicked(object sender, EventArgs e) {
            OnDone(new MenuItemPickedEventArgs {Item = Group});
        }

        public async void saveButton_Clicked(object sender, EventArgs e) {
            await Group.Post(Group);
            OnDone(new MenuItemPickedEventArgs {Item = Group});
        }

        public async void deleteButton_Clicked(object sender, EventArgs e) {
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