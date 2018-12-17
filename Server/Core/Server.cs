using Noname.BitConversion;
using Noname.BitConversion.System;
using Noname.BitConversion.System.Collections.Generic;
using Noname.Net.RPC;
using System.Collections.Generic;
using System.Linq;

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
            _conferences = new List<Conference>();
        }

        public RemoteProcedure<IEnumerable<byte>> SendSoundBytes { get; private set; }
        public RemoteProcedure<IEnumerable<string>> SendOnlineUsers { get; private set; }

        protected override void InitializeLocalProcedures()
        {
            DefineLocalProcedure(false, LogIn, StringBitConverter.UnicodeReliableInstance);
            DefineLocalProcedure(false, GetMicrophoneBytes, ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
            DefineLocalProcedure(false, RequestOnGetOnlineUsers);
        }
        protected override void InitializeRemoteProcedures()
        {
            SendSoundBytes = DefineRemoteProcedure(ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
            SendOnlineUsers = DefineRemoteProcedure(IEnumerableReliableBitConverter.GetInstance(StringBitConverter.ASCIIReliableInstance));
        }

        private void CreateConference(Client creator, IEnumerable<Client> clients) => _conferences.Add(new Conference(creator, clients));

        private void LogIn(Client client, string nickname)
        {
            if (string.IsNullOrEmpty(nickname) || _authorizedClients.Exists(x => x.Nickname == nickname))
                return;
            client.Nickname = nickname;
            _authorizedClients.Add(client);
        }
        private void GetMicrophoneBytes(Client client, IEnumerable<byte> bytes) => UDPCall(SendSoundBytes, bytes, client.Conference.Clients.Where(x => x != client));
        private void RequestOnGetOnlineUsers(Client client) => TCPCall(SendOnlineUsers, _authorizedClients.Where(x => x.Nickname != client.Nickname).Select(x => x.Nickname), client);
    }
}