using System;
using System.Collections.Generic;

namespace ClientApp.Core
{
    public class EntryConferenceEventArgs : EventArgs
    {
        public EntryConferenceEventArgs(int id, string creator, IEnumerable<string> users)
        {
            Id = id;
            Creator = creator ?? throw new ArgumentNullException(nameof(creator));
            Users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public int Id { get; private set; }
        public string Creator { get; private set; }
        public IEnumerable<string> Users { get; private set; }
    }
}