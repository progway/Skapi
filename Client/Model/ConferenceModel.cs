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
    public class ConferenceModel : ModelBase
    {
        private readonly RootModel _rootModel;

        public ConferenceModel(RootModel rootModel, int id, string creator, IEnumerable<string> users)
        {
            _rootModel= rootModel;
            Id = new NotifyingProperty<int>(id);
            Creator = new NotifyingProperty<string>(creator) ?? throw new ArgumentNullException(nameof(creator));
            Nickname = new NotifyingProperty<string>(rootModel.Nickname) ?? throw new ArgumentNullException(nameof(rootModel.Nickname));
            Users = new ObservableCollection<string>(users) ?? throw new ArgumentNullException(nameof(users));
        }

        public NotifyingProperty<int> Id { get; set; }
        public NotifyingProperty<string> Creator { get; set; }
        public NotifyingProperty<string> Nickname { get; set; }
        public ObservableCollection<string> Users { get; set; }

        public void OnChangeMicrophoneState(bool state) => _rootModel.IsMicrophoneActive = state;
    }
}
