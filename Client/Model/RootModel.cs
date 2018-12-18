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

        public bool IsOnline { get; set; }
        public List<string> OnlineUsers { get; set; }

        public void Connect(string address, int port)
        {
            _client = new Client(address, port);
            _client.Connected += _client_Connected;
            _client.Disconnected += _client_Disconnected;
            _client.FailedToConnect += _client_FailedToConnect; ;
            _client.OnLogInError += _client_OnLogInError; 
            _client.OnlineUsersUpdated += _client_OnlineUsersUpdated1;
            _client.Start();
        }

        private void _client_Connected() => IsOnline = true;
        private void _client_Disconnected() => IsOnline = false;
        private void _client_FailedToConnect() => _client.Start();
        private void _client_OnLogInError(object sender, EventArgs e) => throw new NotImplementedException();
        private void _client_OnlineUsersUpdated1(object sender, LogInEventArgs e) => OnlineUsers = e.Users.ToList();
    }
}
