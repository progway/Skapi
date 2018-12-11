using ClientApp.Model;
using Noname.ComponentModel;
using Noname.Windows.MVVM;

namespace ClientApp.ViewModel
{
    public class RootViewModel : ModelBase
    {
        public RootViewModel()
        {
            Model = new RootModel();
            Connect = new Command(OnConnect);
        }

        public RootModel Model { get; }
        public string Address { get; set; }
        public string Port { get; set; }

        public Command Connect { get; }

        private void OnConnect() => Model.Connect(Address, int.Parse(Port));
    }
}
