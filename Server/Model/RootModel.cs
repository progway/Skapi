using Noname.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerApp.Core;

namespace ServerApp.Model
{
    public class RootModel : ModelBase
    {
        private readonly Server _server;

        public RootModel() => _server = new Server(25000);

    }
}
