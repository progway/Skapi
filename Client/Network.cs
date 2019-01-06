using ClientApp.Core;
using ClientApp.Model;
using System;
using System.Linq;
using System.Windows;

namespace ClientApp
{
    static public class Network
    {
        static private Client _client;

        static public event EventHandler<EntryConferenceEventArgs> GetRequestToEntryConference;
        static public event EventHandler<EntryConferenceEventArgs> GetRequestToCreateConference;
        static public event EventHandler<UpdatedConferenceEventArgscs> GetUpdatedConferenceUsers;
        static public event EventHandler<EventArgs> Connected;

        static private void _client_Connected()
        {
            Data.IsOnline = true;
            _client.Authorization(Data.Nickname);
            Connected?.Invoke(null, EventArgs.Empty);
        }
        static private void _client_Disconnected() => Data.IsOnline = false;
        static private void _client_FailedToConnect() => _client.Start();
        static private void _client_OnLogInError(object sender, EventArgs e) => MessageBox.Show("Govno nickname! Please, try to create new nickname");
        static private void _client_OnlineUsersUpdated(object sender, LogInEventArgs e)
        {
            Data.OnlineUsers.Clear();
            foreach (string item in e.Users.Where(x => x != Data.Nickname))
                Data.OnlineUsers.Add(new ClientModel(item));
        }
        static private void _client_GetRequestToEntryConference(object sender, EntryConferenceEventArgs e) => GetRequestToEntryConference?.Invoke(sender, e);
        static private void _client_GetRequestToCreateConference(object sender, EntryConferenceEventArgs e) => GetRequestToCreateConference?.Invoke(sender, e);
        private static void _client_OnGetUpdatedConferenceUsers(object sender, UpdatedConferenceEventArgscs e) => GetUpdatedConferenceUsers?.Invoke(sender, e);

        static public void Connect(string address, int port)
        {
            _client = new Client(address, port);
            _client.Connected += _client_Connected;
            _client.Disconnected += _client_Disconnected;
            _client.FailedToConnect += _client_FailedToConnect; ;
            _client.OnLogInError += _client_OnLogInError;
            _client.OnlineUsersUpdated += _client_OnlineUsersUpdated;
            _client.OnGetRequestToEntryConference += _client_GetRequestToEntryConference;
            _client.OnGetRequestToCreateConference += _client_GetRequestToCreateConference;
            _client.OnGetUpdatedConferenceUsers += _client_OnGetUpdatedConferenceUsers;
            _client.Start();
        }

        static public void SwitchMicrophoneState(bool state) => _client.IsMicrophoneActive = state;
        static public void SwitchSoundState(bool state) => _client.IsSoundActive = state;
        static public void ResponceOnEntryConference(int id, bool state) => _client.TCPCall(_client.ResponseOnEntryConference, id, state);
        static public void Call(string client) => _client.Call(client);
        static public void Stop()
        {
            if (_client != null)
                _client.Stop();
        }
        static public void ExitConference()
        {
            _client.MicrophoneOff();
            _client.TCPCall(_client.ExitConference);
        }
        static public void AddUserToConference(string user) => _client.TCPCall(_client.AddUserToConference, user);
    }
}
