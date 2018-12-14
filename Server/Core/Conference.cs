using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Core
{
    public class Conference
    {
        public Conference(List<Client> clients) => Clients = clients ?? throw new ArgumentNullException(nameof(clients));

        public List<Client> Clients { get; private set; }
    }
}
