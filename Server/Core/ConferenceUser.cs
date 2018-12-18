using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerApp.Core
{
    public class ConferenceUser
    {
        public ConferenceUser(Client client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            InConference = false;
        }

        public Client Client { get; private set; }
        public bool InConference { get; set; }

        public void SendRequestToEnterConference(ConferenceUser creator, List<ConferenceUser> conferenceUsers) => NetworkManager.SendRequestToEnterConference(Client, creator.Client, conferenceUsers.Select(x => x.Client));
    }
}
