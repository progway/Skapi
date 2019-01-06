using ClientApp.Model;
using Noname.ComponentModel;
using Noname.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModel
{
    public class MenuVM : ModelBase
    {
        public MenuVM() => UserCall = new Command<ClientModel>(OnUserCall);

        public ObservableCollection<ClientModel> OnlineUsers => Data.OnlineUsers;

        public Command<ClientModel> UserCall { get; }

        private void OnUserCall(ClientModel clientModel) => Network.Call(clientModel.Name.Value);
    }
}
