using ClientApp.Core;
using Noname.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClientApp.Model
{
    public class ConferenceModel : ModelBase
    {
        public ConferenceModel(int id, string creator, IEnumerable<string> users)
        {
            Id = new NotifyingProperty<int>(id);
            Creator = new NotifyingProperty<string>(creator) ?? throw new ArgumentNullException(nameof(creator));
            Nickname = new NotifyingProperty<string>(Data.Nickname) ?? throw new ArgumentNullException(nameof(Data.Nickname));
            Users = new ObservableCollection<string>(users) ?? throw new ArgumentNullException(nameof(users));
            Network.GetUpdatedConferenceUsers += Network_GetUpdatedConferenceUsers;
        }

        public NotifyingProperty<int> Id { get; set; }
        public NotifyingProperty<string> Creator { get; set; }
        public NotifyingProperty<string> Nickname { get; set; }
        public ObservableCollection<string> Users { get; set; }

        private void Network_GetUpdatedConferenceUsers(object sender, UpdatedConferenceEventArgscs e)
        {
            Users.Clear();
            foreach (string item in e.Users)
                Users.Add(item);
        }
    }
}
