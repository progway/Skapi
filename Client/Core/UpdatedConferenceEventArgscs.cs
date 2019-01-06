using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public class UpdatedConferenceEventArgscs : EventArgs
    {
        public IEnumerable<string> Users { get; private set; }

        public UpdatedConferenceEventArgscs(IEnumerable<string> users) => Users = users;
    }
}
