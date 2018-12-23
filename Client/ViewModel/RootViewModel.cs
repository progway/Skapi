using ClientApp.Model;
using Noname.ComponentModel;
using Noname.Windows.MVVM;
using System.Windows;

namespace ClientApp.ViewModel
{
    public class RootViewModel : ModelBase
    {
        public RootViewModel()
        {
            Model = new RootModel();
            Connect = new Command(OnConnect);
            UserCall = new Command<ClientModel>(OnUserCall);
        }

        public RootModel Model { get; }

        public string Address { get; set; }
        public string Port { get; set; }
        public Command Connect { get; }
        public Command<ClientModel> UserCall { get;}

        private void OnConnect() => Model.Connect(Address, int.Parse(Port));
        private void OnUserCall(ClientModel client)
        {
            MessageBox.Show("123");
        }
    }
}
