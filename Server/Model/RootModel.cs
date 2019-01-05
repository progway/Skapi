using Noname.ComponentModel;
using ServerApp.Core;

namespace ServerApp.Model
{
    public class RootModel : ModelBase
    {
        public RootModel() => NetworkManager.Initialize(new Server(25000));

        public void Stop() => NetworkManager.Stop();
    }
}
