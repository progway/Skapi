using ClientApp.Core;
using ClientApp.View;
using Noname.ComponentModel;
using Noname.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientApp.ViewModel
{
    public class RequestVM : ModelBase
    {
        static public void Invoke(EntryConferenceEventArgs param) => new Request(param).ShowDialog();

        private EventHandler _closeWindow;

        public RequestVM() { }
        public RequestVM(EntryConferenceEventArgs param, EventHandler closeWindow)
        {
            Id = param.Id;
            Creator = param.Creator;
            Users = param.Users.ToList();
            _closeWindow = closeWindow;
            Agreement = new Command(OnAgreement);
            Disagreement = new Command(OnDisagreement);
        }

        public int Id { get; set; }
        public string Creator { get; set; }
        public List<string> Users { get; set; }
        public Command Agreement { get; }
        public Command Disagreement { get; }

        private void OnAgreement() { Network.ResponceOnEntryConference(Id, true); _closeWindow?.Invoke(this, EventArgs.Empty); }
        private void OnDisagreement() { Network.ResponceOnEntryConference(Id, false); _closeWindow?.Invoke(this, EventArgs.Empty); }
    }
}
