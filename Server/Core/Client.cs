using Noname.Net.RPC;
using System;

namespace ServerApp.Core
{
    public class Client : RPCClientConnectionProvider
    {
        private bool _isSoundMute;

        public Client() { }

        public string Nickname;
        public Conference Conference;
        public bool IsSoundMute { get => _isSoundMute; set { _isSoundMute = value;  IsSoundMuteSwitched?.Invoke(this, new IsSoundeMuteEventArgs(value)); } }

        public event EventHandler<IsSoundeMuteEventArgs> IsSoundMuteSwitched;
    }
    public class IsSoundeMuteEventArgs : EventArgs
    {
        public bool IsSoundMute { get; private set; }
        public IsSoundeMuteEventArgs(bool isSoundMute) => IsSoundMute = isSoundMute;
    }
}
