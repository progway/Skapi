using Noname.ComponentModel;
using ServerApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.ViewModel
{
    class RootViewModel : ModelBase
    {
        public RootModel Model { get; }

        public RootViewModel()
        {
            Model = new RootModel();
        }
    }
}
