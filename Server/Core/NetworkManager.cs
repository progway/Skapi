﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerApp.Core
{
    public static class NetworkManager
    {
        static private Server _server;

        static public void Initialize(Server server) => _server = server ?? throw new ArgumentNullException(nameof(server));

        static public void SendRequestToEnterConference(int id, Client recipient, Client creator, IEnumerable<Client> clients) => _server.TCPCall(_server.SendRequestToEntryConference, id, creator.Nickname, clients.Select(x => x.Nickname), recipient);
        static public void SendSoundBytes(IEnumerable<byte> bytes, IEnumerable<Client> clients) => _server.UDPCall(_server.SendSoundBytes, bytes, clients);
        static public void UpdateConferenceUsers(int idConference) => _server.UpdateConferenceUsers(idConference);
        static public void Stop() => _server.Stop();
    }
}
