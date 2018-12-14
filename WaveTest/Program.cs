using NAudio.Codecs;
using NAudio.Wave;
using System;
using System.Threading.Tasks;

namespace WaveTest
{
    internal class Program
    {
        static private WaveFormat _waveFormat = new WaveFormat(44100, 1);
        static private BufferedWaveProvider _bufferedWaveProvider;
        static private WaveInEvent _waveIn;
        static private WaveOutEvent _waveOut;
        private static readonly G722CodecState _g722CodecState = new G722CodecState(48000, G722Flags.SampleRate8000);

        private static void Main(string[] args)
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

            Console.ReadKey();
        }

        static private void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {

        }
        static private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            G722Codec codec = new G722Codec();
            short[] newData = new short[e.BytesRecorded / 2];
            for (int i = 0; i < e.BytesRecorded/2; i++)
            {
                newData[i] = (short)(e.Buffer[2 * i] >> 8);
                newData[i] += e.Buffer[2 * i + 1];
            }
            byte[] buffer = new byte[newData.Length*2];
            //for (int i = 0; i < buffer.Length; i++)
                //buffer[i] = ALawEncoder.LinearToALawSample(newData[i]);
            
            codec.Encode(_g722CodecState, buffer, newData, newData.Length);
            _bufferedWaveProvider.AddSamples(buffer, 0, buffer.Length);
        }
    }
}
