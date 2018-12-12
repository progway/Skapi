using NAudio.Mixer;
using NAudio.Wave;
using NAudio.Wave.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveTest
{
    class Program
    {
        static private WaveFormat _waveFormat = new WaveFormat(44100, 1);
        static private BufferedWaveProvider _bufferedWaveProvider;
        static private WaveInEvent _waveIn;
        static private WaveOutEvent _waveOut;

        static void Main(string[] args)
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
            int sum = 0;
            for (int i = 0; i < e.BytesRecorded; i++)
                sum +=e.Buffer[i];
            Console.WriteLine(e.BytesRecorded + " " + sum);
            _bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }
    }
}
