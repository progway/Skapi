using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Core
{
    public class Conference
    {
        public Conference(Client creator, IEnumerable<Client> clients)
        {
            Creator = creator;
            Clients = clients.ToList() ?? throw new ArgumentNullException(nameof(clients));
        }

        public Client Creator;
        public List<Client> Clients { get; private set; }

        public bool AddClient(Client client)
        {
            if (Clients.Contains(client))
                return false;

            Clients.Add(client);
            return true;
        }
        public bool RemoveClient(Client client)
        {
            if (Clients.Contains(client))
                return false;

            Clients.Add(client);
            return true;
        }
    }
}
