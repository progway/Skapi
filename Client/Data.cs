using ClientApp.Model;
using System.Collections.ObjectModel;

namespace ClientApp
{
    static public class Data
    {
        static Data() => OnlineUsers = new ObservableCollection<ClientModel>();

        static private bool _isMicrophoneActive;
        static private bool _isSoundActive;

        static public bool IsOnline { get; set; }
        static public string Nickname { get; set; }
        static public ObservableCollection<ClientModel> OnlineUsers { get; set; }
        static public bool IsMicrophoneActive { get => _isMicrophoneActive; set { _isMicrophoneActive = value; Network.SwitchMicrophoneState(value); } }
        static public bool IsSoundActive { get => _isSoundActive; set { _isSoundActive = value; Network.SwitchSoundState(value); } }
    }
}
