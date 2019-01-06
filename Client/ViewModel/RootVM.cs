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
    public class RootVM : ModelBase
    {
        static public RootVM This;

        public RootVM()
        {
            This = this;
            CurrentContentVM = new NotifyingProperty<ModelBase>(new ConnectionVM());
            Network.GetRequestToEntryConference += Model_OnGetRequestToEntryConference;
            Network.GetRequestToCreateConference += Model_OnGetRequestToCreateConference;
            Network.Connected += Network_Connected;
        }

        public NotifyingProperty<ModelBase> CurrentContentVM { get; set; }

        private void Network_Connected(object sender, System.EventArgs e) => CurrentContentVM.Value = new MenuVM();
        private void Model_OnGetRequestToEntryConference(object sender, EntryConferenceEventArgs e) => RequestVM.Invoke(e);
        private void Model_OnGetRequestToCreateConference(object sender, EntryConferenceEventArgs e) => CurrentContentVM.Value = new ConferenceVM(new ConferenceModel(e.Id, e.Creator, e.Users));

        public void ExitConference()
        {
            CurrentContentVM.Value = new MenuVM();
            Network.ExitConference();
        }
    }
}
