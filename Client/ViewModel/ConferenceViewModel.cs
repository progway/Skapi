using ClientApp.Core;
using ClientApp.Model;
using ClientApp.View;
using Noname.ComponentModel;
using Noname.Windows.MVVM;
using System.Collections.ObjectModel;
using System.Windows;

namespace ClientApp.ViewModel
{
    public class ConferenceViewModel : ModelBase
    {
        private bool _microphoneState;

        public ConferenceViewModel() { }
        public ConferenceViewModel(ConferenceModel conferenceModel)
        {
            _microphoneState = true;
            Model = conferenceModel;
            ChangeMicrophoneState = new Command(OnChangeMicrophoneState);
        }

        public Command ChangeMicrophoneState { get; }
       
        public ConferenceModel Model { get; }

        private void OnChangeMicrophoneState()
        {
            _microphoneState = !_microphoneState;
            Model.OnChangeMicrophoneState(_microphoneState);
        }
    }
}