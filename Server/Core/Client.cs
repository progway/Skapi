using Noname.Net.RPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Core
{
    public class Client : RPCClientConnectionProvider
    {
        public Client() { }

        public string Login { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
    }
}
