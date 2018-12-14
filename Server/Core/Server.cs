using NAudio.Wave;
using Noname.BitConversion;
using Noname.BitConversion.System;
using Noname.BitConversion.System.Collections.Generic;
using Noname.Net.RPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Core
{
    public class Server : RPCServer<Client>
    {
        private readonly List<Client> _authorizedClients;
        private readonly List<Conference> _conferences;

        public Server(int port) : base(port)
        {
            Start();
            _authorizedClients = new List<Client>();
        }

        public RemoteProcedure<IEnumerable<byte>> SendBytes { get; private set; }

        protected override void InitializeLocalProcedures()
        {
            DefineLocalProcedure(false, LogIn, StringBitConverter.UnicodeReliableInstance);
            DefineLocalProcedure(false, GetBytes, ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
        }
        protected override void InitializeRemoteProcedures()
        {
            SendBytes = DefineRemoteProcedure(ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
        }

        private void LogIn(Client client, string nickname)
        {
            if (string.IsNullOrEmpty(nickname) || _authorizedClients.Exists(x => x.Nickname == nickname))
                return;
            client.Nickname = nickname;
            _authorizedClients.Add(client);
        }
        private void GetBytes(Client client, IEnumerable<byte> bytes) { }
    }
}