using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerApp.Core
{
    public class Conference
    {
        public Conference(Client creator, IEnumerable<Client> clients)
        {
            Creator = creator;
            Clients = clients.Select(client => new ConferenceUser(client)).ToList() ?? throw new ArgumentNullException(nameof(clients));
            Clients.Add(new ConferenceUser(creator));
        }

        public Client Creator;
        public List<ConferenceUser> Clients { get; private set; }

        public bool AddClient(Client client)
        {
            if (Clients.Select(x => x.Client).Contains(client))
                return false;

            Clients.Add(new ConferenceUser(client));
            return true;
        }
        public void RemoveClient(Client client) => Clients.Remove(Clients.First(x => x.Client == client));
    }
}
