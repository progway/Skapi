using ClientApp.Core;
using ClientApp.Model;
using ClientApp.View;
using Noname.ComponentModel;
using Noname.Windows.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace ClientApp.ViewModel
{
    public class ConferenceVM : ModelBase
    {
        private bool _microphoneState;
        private bool _soundState;

        public ConferenceVM() { }
        public ConferenceVM(ConferenceModel conferenceModel)
        {
            Model = conferenceModel;
            _microphoneState = true;
            _soundState = true;
            Data.IsMicrophoneActive = _microphoneState;
            Data.IsSoundActive = _soundState;
            ChangeMicrophoneState = new Command(OnChangeMicrophoneState);
            ChangeSoundState = new Command(OnChangeSoundState);
            AddUser = new Command<ClientModel>(OnAddUser);
            Exit = new Command(OnExit);
        }

        public ConferenceModel Model { get; }
        public Command Exit { get; }
        public Command ChangeMicrophoneState { get; }
        public Command ChangeSoundState { get; }
        public Command<ClientModel> AddUser { get; }
        public ObservableCollection<ClientModel> OnlineUsers => Data.OnlineUsers;

        private void OnAddUser(ClientModel clientModel) => Network.AddUserToConference(clientModel.Name.Value);
        private void OnChangeMicrophoneState()
        {
            _microphoneState = !_microphoneState;
            Data.IsMicrophoneActive = _microphoneState;
        }
        private void OnChangeSoundState()
        {
            _soundState = !_soundState;
            Data.IsSoundActive = _soundState;
        }
        private void OnExit()
        {
            _microphoneState = false;
            _soundState = false;
            Data.IsMicrophoneActive = _microphoneState;
            Data.IsSoundActive = _soundState;
            RootVM.This.ExitConference();
        }
    }
}