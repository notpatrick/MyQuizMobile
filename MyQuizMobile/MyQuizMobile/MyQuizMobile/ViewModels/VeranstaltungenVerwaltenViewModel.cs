using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyQuizMobile.DataModel;
using MYQuizMobile;
using Xamarin.Forms;

namespace MyQuizMobile
{
    public class VeranstaltungenVerwaltenViewModel
    {
        private readonly Networking _networking;
        public ObservableCollection<Group> Groups { get; set; }


        public VeranstaltungenVerwaltenViewModel()
        {
            _networking = DependencyService.Get<Networking>();
            Groups = new ObservableCollection<Group>();
            Init();
        }

        public void Init()
        {

            ThreadPool.QueueUserWorkItem(async (o) => {
                var groups = await GetAllGroups();
                foreach (var g in groups)
                {
                    Groups.Add(g);
                }
            });
        }

        private async Task<List<Group>> GetAllGroups()
        {
            var groups = await _networking.Get<List<Group>>("api/groups/");
            foreach (var g in groups)
            {
                g.SetDisplayText();
            }
            return groups;
        }

        public void addButton_Clicked(object sender, EventArgs e)
        {
        }
    }
}
