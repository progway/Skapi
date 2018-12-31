using ClientApp.Core;
using ClientApp.Model;
using ClientApp.View;
using Noname.ComponentModel;
using Noname.Windows.MVVM;

namespace ClientApp.ViewModel
{
    public class RootViewModel : ModelBase
    {
        public RootViewModel()
        {
            Model = new RootModel();
            Model.OnGetRequestToEntryConference += Model_OnGetRequestToEntryConference;
            Connect = new Command(OnConnect);
            UserCall = new Command<ClientModel>(OnUserCall);
        }

        private void Model_OnGetRequestToEntryConference(object sender, EntryConferenceEventArgs e) => new ConferenceView(new ConferenceModel(Model, e.Id, e.Creator, e.Users)).ShowDialog();

        public RootModel Model { get; }

        public string Address { get; set; }
        public string Port { get; set; }
        public Command Connect { get; }
        public Command<ClientModel> UserCall { get; }

        private void OnConnect() => Model.Connect(Address, int.Parse(Port));
        private void OnUserCall(ClientModel client) => Model.Call(client);
    }
}
