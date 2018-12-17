using ClientApp.Core;
using Noname.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Model
{
    public class RootModel : ModelBase
    {
        private Client _client;

        public RootModel() { } 

        private void _client_Connected() => _client.MicrophoneOn();

        public void Connect(string address, int port)
        {
            _client = new Client(address, port);
            _client.Connected += _client_Connected;
            _client.Start();
        }
    }
}
