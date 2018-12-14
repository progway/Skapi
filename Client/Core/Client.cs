using NAudio.Wave;
using Noname.BitConversion;
using Noname.BitConversion.System;
using Noname.BitConversion.System.Collections.Generic;
using Noname.Net.RPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public class Client : RPCClient
    {
        private WaveFormat _waveFormat = new WaveFormat(44100, 1);
        private BufferedWaveProvider _bufferedWaveProvider;
        private WaveInEvent _waveIn;
        private WaveOutEvent _waveOut;

        public Client(string ipAddress, int port) : base(ipAddress, port) { }

        public RemoteProcedure<string> LogIn { get; private set; }
        public RemoteProcedure<IEnumerable<byte>> SendMicrophoneBytes { get; private set; }

        protected override void InitializeLocalProcedures()
        {
            DefineLocalProcedure(false, GetSoundBytes, ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
        }
        protected override void InitializeRemoteProcedures()
        {
            LogIn = DefineRemoteProcedure(StringBitConverter.UnicodeReliableInstance);
            SendMicrophoneBytes = DefineRemoteProcedure(ReliableBitConverter.GetInstance(IEnumerableVariableLengthBitConverter.GetInstance(ByteBitConverter.Instance)));
        }

        private void GetSoundBytes(IEnumerable<byte> bytes) => _bufferedWaveProvider.AddSamples(bytes.ToArray(), 0, bytes.Count());

        private void WaveIn_RecordingStopped(object sender, StoppedEventArgs e) => throw new NotImplementedException();
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e) => UDPCall(SendMicrophoneBytes, e.Buffer);

        public void Run()
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
                DesiredLatency = 100,
            };
            _waveOut.Init(_bufferedWaveProvider);

            Task task = new Task(() => { _waveIn.StartRecording(); while (true) { } });
            task.Start();

            Task task2 = new Task(() => { _waveOut.Play(); while (true) { } });
            task2.Start();
        }
    }

}
