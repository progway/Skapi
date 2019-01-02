﻿using ClientApp.Core;
using Noname.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ClientApp.Model
{
    public class RootModel : ModelBase
    {
        private Client _client;

        public RootModel() => OnlineUsers = new ObservableCollection<ClientModel>();

        public bool IsOnline { get; set; }
        public string Nickname { get; set; }
        public ObservableCollection<ClientModel> OnlineUsers { get; set; }
        public event EventHandler<EntryConferenceEventArgs> OnGetRequestToEntryConference;
        public event EventHandler<EntryConferenceEventArgs> OnGetRequestToCreateConference;
        public bool IsMicrophoneActive { get => _client.IsMicrophoneActive; set => _client.IsMicrophoneActive = value; }

        public void Connect(string address, int port)
        {
            _client = new Client(address, port);
            _client.Connected += _client_Connected;
            _client.Disconnected += _client_Disconnected;
            _client.FailedToConnect += _client_FailedToConnect; ;
            _client.OnLogInError += _client_OnLogInError;
            _client.OnlineUsersUpdated += _client_OnlineUsersUpdated;
            _client.OnGetRequestToEntryConference += _client_OnGetRequestToEntryConference;
            _client.OnGetRequestToCreateConference += _client_OnGetRequestToCreateConference;
            _client.Start();
        }

        public void Call(ClientModel clientModel) => _client.Call(clientModel.Name.Value);
        public void ResponceOnEntryConference(int id, bool state) => _client.TCPCall(_client.ResponseOnEntryConference, id, state);

        private void _client_Connected()
        {
            IsOnline = true;
            _client.Authorization(Nickname);
        }
        private void _client_Disconnected() => IsOnline = false;
        private void _client_FailedToConnect() => _client.Start();
        private void _client_OnLogInError(object sender, EventArgs e) => MessageBox.Show("Govno nickname! Please, try to create new nickname");
        private void _client_OnlineUsersUpdated(object sender, LogInEventArgs e)
        {
            OnlineUsers.Clear();
            foreach (string item in e.Users.Where(x => x != Nickname))
                OnlineUsers.Add(new ClientModel(item));
        }
        private void _client_OnGetRequestToEntryConference(object sender, EntryConferenceEventArgs e) => OnGetRequestToEntryConference?.Invoke(this, e);
        private void _client_OnGetRequestToCreateConference(object sender, EntryConferenceEventArgs e) => OnGetRequestToCreateConference?.Invoke(this, e);
    }
}
