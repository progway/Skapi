using ClientApp.Core;
using Noname.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Model
{
    public class ClientModel : ModelBase
    {
        public ClientModel(string name) => Name = new NotifyingProperty<string>(name) ?? throw new ArgumentNullException(nameof(name));

        public NotifyingProperty<string> Name { get; set; }
    }
}
