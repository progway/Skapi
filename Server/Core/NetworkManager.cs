using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Core
{
    public static class NetworkManager
    {
        static private Server _server;

        static public void Initialize(Server server) => _server = server ?? throw new ArgumentNullException(nameof(server));

        static public void SendRequestToEnterConference(Client recipient, Client creator, IEnumerable<Client> clients) => _server.TCPCall(_server.SendRequestToEnterConference, creator.Nickname, clients.Select(x => x.Nickname), recipient);
    }
}
