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

        public RemoteProcedure LogInError { get; private set; }
        public RemoteProcedure<IEnumerable<string>> SendOnlineUsers { get; private set; }
        public RemoteProcedure<IEnumerable<byte>> SendSoundBytes { get; private set; }
        public RemoteProcedure<string, IEnumerable<string>> SendRequestToEnterConference { get; private set; }

        protected override void InitializeLocalProcedures()
        {
            DefineLocalProcedure(false, LogIn, StringBitConverter.ASCIIReliableInstance);
            DefineLocalProcedure(false, GetMicrophoneBytes, ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
            DefineLocalProcedure(false, RequestOnGetOnlineUsers);
            DefineLocalProcedure(false, SwitchSoundState, BooleanBitConverter.Instance);
            DefineLocalProcedure(false, RequestOnCreateConference, IEnumerableReliableBitConverter.GetInstance(StringBitConverter.ASCIIReliableInstance));
        }
        protected override void InitializeRemoteProcedures()
        {
            LogInError = DefineRemoteProcedure();
            SendOnlineUsers = DefineRemoteProcedure(IEnumerableReliableBitConverter.GetInstance(StringBitConverter.ASCIIReliableInstance));
            SendSoundBytes = DefineRemoteProcedure(ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
            SendRequestToEnterConference = DefineRemoteProcedure(StringBitConverter.ASCIIReliableInstance, IEnumerableReliableBitConverter.GetInstance(StringBitConverter.ASCIIReliableInstance));
        }

        private void CreateConference(Client creator, IEnumerable<Client> clients) => _conferences.Add(new Conference(creator, clients));

        private void LogIn(Client client, string nickname)
        {
            if (string.IsNullOrEmpty(nickname) || _authorizedClients.Exists(x => x.Nickname == nickname))
            { TCPCall(LogInError, client); return; }
            client.Nickname = nickname;
            _authorizedClients.Add(client);
            TCPCall(SendOnlineUsers, _authorizedClients.Select(x => x.Nickname));
        }
        private void GetMicrophoneBytes(Client client, IEnumerable<byte> bytes) => UDPCall(SendSoundBytes, bytes, client.Conference.Clients.Select(x => x.Client).Where(x => x != client));
        private void RequestOnGetOnlineUsers(Client client) => TCPCall(SendOnlineUsers, _authorizedClients.Where(x => x.Nickname != client.Nickname).Select(x => x.Nickname), client);
        private void SwitchSoundState(Client client, bool state) => client.IsSoundMute = state;
        private void RequestOnCreateConference(Client client, IEnumerable<string> users)
        {
            if (client.Conference == null)
                _conferences.Add(new Conference(client, _authorizedClients.Where(x => users.Contains(x.Nickname))));
        }
    }
}