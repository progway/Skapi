using ILibrary.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Midi;

namespace ServerApp.Core
{
    public class Conference
    {
        private readonly static IDController _idController;

        static Conference() => _idController = new IDController();

        public Conference(Client creator, IEnumerable<Client> clients, out int id)
        {
            id = _idController.GetID();
            Id = id;
            Creator = new ConferenceUser(creator);
            Clients = clients.Select(client => new ConferenceUser(client)).ToDictionary(x=> x.Client) ?? throw new ArgumentNullException(nameof(clients));
            foreach (ConferenceUser user in Clients.Values)
                user.SendRequestToEnterConference(Id, Creator, Clients.Values.ToList());
            Clients.Add(Creator.Client, Creator);
        }

        public ConferenceUser Creator;
        public Dictionary<Client, ConferenceUser> Clients { get; private set; }
        public int Id { get; private set; }

        public bool AddClient(Client client)
        {
            if (Clients.Keys.Contains(client))
                return false;

            Clients.Add(client, new ConferenceUser(client));
            return true;
        }
        public void RemoveClient(Client client) => Clients.Remove(client);
        public void GetMicrophoneBytes(Client client, IEnumerable<byte> bytes)
        {
            //UDPCall(SendSoundBytes, bytes, client.Conference.Clients.Values.Select(x => x.Client).Where(x => x != client));
        }

        public void Destroy() => _idController.ReturnID(Id);
    }
}
