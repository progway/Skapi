using Noname.ComponentModel;
using Noname.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModel
{
    public class ConnectionVM : ModelBase
    {
        public ConnectionVM() => Connect = new Command(OnConnect);

        public string Nickname { get => Data.Nickname; set => Data.Nickname = value; }
        public string Address { get; set; }
        public string Port { get; set; }
        public Command Connect { get; }

        private void OnConnect() => Network.Connect(Address, int.Parse(Port));
    }
}
