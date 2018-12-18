using System;
using System.Collections.Generic;

namespace ClientApp.Core
{
    public class LogInEventArgs : EventArgs
    {
        public IEnumerable<string> Users { get; private set; }

        public LogInEventArgs(IEnumerable<string> users) => Users = users;
    }
}