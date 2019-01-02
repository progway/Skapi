﻿using Noname.BitConversion;
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
        private readonly Dictionary<int, Conference> _conferences;

        public Server(int port) : base(port)
        {
            Start();
            _authorizedClients = new List<Client>();
            _conferences = new Dictionary<int, Conference>();
        }

        public RemoteProcedure LogInError { get; private set; }
        public RemoteProcedure<IEnumerable<string>> SendOnlineUsers { get; private set; }
        public RemoteProcedure<IEnumerable<byte>> SendSoundBytes { get; private set; }
        public RemoteProcedure<int, string, IEnumerable<string>> SendRequestToEntryConference { get; private set; }
        public RemoteProcedure<int, string, IEnumerable<string>> SendRequestToCreateConference { get; private set; }

        protected override void InitializeLocalProcedures()
        {
            DefineLocalProcedure(true, LogIn, StringBitConverter.ASCIIReliableInstance);
            DefineLocalProcedure(true, GetMicrophoneBytes, ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
            DefineLocalProcedure(true, RequestOnGetOnlineUsers);
            DefineLocalProcedure(true, SwitchSoundState, BooleanBitConverter.Instance);
            DefineLocalProcedure(true, RequestOnCreateConference, IEnumerableReliableBitConverter.GetInstance(StringBitConverter.ASCIIReliableInstance));
            DefineLocalProcedure(true, ResponseOnEntryConference, Int32BitConverter.Instance, BooleanBitConverter.Instance);
        }
        protected override void InitializeRemoteProcedures()
        {
            LogInError = DefineRemoteProcedure();
            SendOnlineUsers = DefineRemoteProcedure(IEnumerableReliableBitConverter.GetInstance(StringBitConverter.ASCIIReliableInstance));
            SendSoundBytes = DefineRemoteProcedure(ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
            SendRequestToEntryConference = DefineRemoteProcedure(Int32BitConverter.Instance, StringBitConverter.ASCIIReliableInstance, IEnumerableReliableBitConverter.GetInstance(StringBitConverter.ASCIIReliableInstance));
            SendRequestToCreateConference = DefineRemoteProcedure(Int32BitConverter.Instance, StringBitConverter.ASCIIReliableInstance, IEnumerableReliableBitConverter.GetInstance(StringBitConverter.ASCIIReliableInstance));
        }

        private void CreateConference(Client creator, IEnumerable<Client> clients, out int id)
        {
            Conference conference = new Conference(creator, clients, out id);
            creator.Conference = conference;
            _conferences.Add(id, conference);
        }

        private void LogIn(Client client, string nickname)
        {
            if (string.IsNullOrEmpty(nickname) || _authorizedClients.Exists(x => x.Nickname == nickname))
            { TCPCall(LogInError, client); return; }
            client.Nickname = nickname;
            _authorizedClients.Add(client);
            TCPCall(SendOnlineUsers, _authorizedClients.Select(x => x.Nickname));
        }
        private void GetMicrophoneBytes(Client client, IEnumerable<byte> bytes) => client.Conference.GetMicrophoneBytes(client, bytes);
        private void RequestOnGetOnlineUsers(Client client) => TCPCall(SendOnlineUsers, _authorizedClients.Where(x => x.Nickname != client.Nickname).Select(x => x.Nickname), client);
        private void SwitchSoundState(Client client, bool state) => client.IsSoundMute = state;
        private void RequestOnCreateConference(Client client, IEnumerable<string> users)
        {
            if (client.Conference == null)
            {
                CreateConference(client, _authorizedClients.Where(x => users.Contains(x.Nickname)), out int id);
                TCPCall(SendRequestToCreateConference, id, client.Nickname, users, client);
            }
        }
        private void ResponseOnEntryConference(Client client, int id, bool state)
        {
            if (state == true)
            {
                if (_conferences.TryGetValue(id, out Conference conference))
                {
                    if (conference.Clients.TryGetValue(client, out ConferenceUser user))
                    {
                        client.Conference = conference;
                        user.InConference = true;
                        TCPCall(SendRequestToCreateConference, conference.Id, conference.Creator.Client.Nickname, conference.Clients.Keys.Select(x => x.Nickname), client);
                    }
                }
            }
            else
            {
                if (_conferences.TryGetValue(id, out Conference conference))
                    conference.Clients.Remove(client);
            }
        }
    }
}