using NAudio.Wave;
using Noname.BitConversion;
using Noname.BitConversion.System;
using Noname.BitConversion.System.Collections.Generic;
using Noname.Net.RPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Core
{
    public class Server : RPCServer<Client>
    {
        private readonly List<Client> _authorizedClients;
        private WaveFormat _waveFormat = new WaveFormat(50000, 1);
        private BufferedWaveProvider _bufferedWaveProvider;
        private WaveInEvent _waveIn;
        private WaveOutEvent _waveOut;

        public Server(int port) : base(port)
        {
            ClientConnected += Server_ClientConnected;
            _authorizedClients = new List<Client>();
        }

        private void Server_ClientConnected(Client obj)
        {
            _bufferedWaveProvider = new BufferedWaveProvider(_waveFormat);

            _waveIn = new WaveInEvent()
            {
                DeviceNumber = 0,
                WaveFormat = _waveFormat,
                BufferMilliseconds = 100,
            };
            _waveIn.DataAvailable += WaveIn_DataAvailable;
            _waveIn.RecordingStopped += WaveIn_RecordingStopped;

            _waveOut = new WaveOutEvent()
            {
                DeviceNumber = 0,
            };
            _waveOut.Init(_bufferedWaveProvider);

            Task task = new Task(() => { _waveIn.StartRecording(); while (true) { } });
            task.Start();

            Task task2 = new Task(() => { _waveOut.Play(); while (true) { } });
            task2.Start();
        }

        public RemoteProcedure<IEnumerable<byte>> SendBytes { get; private set; }

        protected override void InitializeLocalProcedures()
        {
            DefineLocalProcedure(false, SignIn, StringBitConverter.UnicodeReliableInstance, StringBitConverter.UnicodeReliableInstance, StringBitConverter.UnicodeReliableInstance);
            DefineLocalProcedure(false, LogIn, StringBitConverter.UnicodeReliableInstance, StringBitConverter.UnicodeReliableInstance);
            DefineLocalProcedure(false, GetBytes, ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
        }
        protected override void InitializeRemoteProcedures()
        {
            SendBytes = DefineRemoteProcedure(ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
        }

        private void WaveIn_RecordingStopped(object sender, StoppedEventArgs e) => throw new NotImplementedException();
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e) => UDPCall(SendBytes, e.Buffer);

        private void SignIn(Client client, string login, string password, string nickname)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nickname))
                return;
            if (_authorizedClients.Exists(x => x.Login == login) || _authorizedClients.Exists(x => x.Nickname == nickname))
                return;
            client.Login = login;
            client.Password = password;
            client.Nickname = nickname;
            _authorizedClients.Add(client);
        }
        private void LogIn(Client client, string login, string password)
        {
            if (!_authorizedClients.Exists(x => x.Login == login))
                return;
            Client oldClient = _authorizedClients.First(x => x.Login == login);
            if (oldClient.Password == password)
                oldClient = client;
        }
        private void GetBytes(Client client, IEnumerable<byte> bytes)
        {
            _bufferedWaveProvider.AddSamples(bytes.ToArray(), 0, bytes.Count());
        }
    }
}
