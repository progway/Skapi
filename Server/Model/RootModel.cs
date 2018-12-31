using Noname.ComponentModel;
using ServerApp.Core;

namespace ServerApp.Model
{
    public class RootModel : ModelBase
    {
        private readonly Server _server;

        public RootModel()
        {
            _server = new Server(25000);
            NetworkManager.Initialize(_server);
        }
    }
}
