using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace Jaxx.Net.Cobaka.NAudioWrapper
{
    public class NAudioHandler : IAudioHandler
    {
        private WaveFileWriter _writer;
        private WasapiCapture _audioIn;
        private readonly INoiseDetectorOptions _noiseDetectionOptions;
        private readonly Timer _recordTimer;
        private bool _isStopAndDisposeRequested;
        public NAudioHandler(INoiseDetectorOptions options)
        {
            _noiseDetectionOptions = options;
            if (_noiseDetectionOptions.ListenOnStartup) StartListen();
            _recordTimer = new Timer { AutoReset = true };
            _recordTimer.Elapsed += RecordTimer_Elapsed;
        }

        public void StartListen()
        {
            _audioIn = new WasapiCapture();
            _audioIn.DataAvailable += AudioInDataAvailable;
            _audioIn.RecordingStopped += AudioInRecordingStopped;
            PeakValue = 0;
            _audioIn.StartRecording();
            IsListening = true;
            OnAudioEventAvailable(new AudioEventArgs { State = AudioRecordState.ListeningStarted, Information = "Listening started." });

        }
        private void RecordTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            StopRecord();
        }

        public event EventHandler<AudioEventArgs> AudioEventAvailable;

        protected virtual void OnAudioEventAvailable(AudioEventArgs e)
        {
            EventHandler<AudioEventArgs> handler = AudioEventAvailable;
            handler?.Invoke(this, e);
        }

        public bool IsRecording { get; private set; }
        public float PeakValue { get; private set; }
        public List<string> DeviceList
        {
            get
            {
                return GetDevices();
            }
        }

        public bool IsListening { get; private set; }

        private List<string> GetDevices()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var endpoints = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            var endpointList = new List<string>();

            foreach (var endpoint in endpoints)
            {
                endpointList.Add(endpoint.DeviceFriendlyName);
            }

            return endpointList;
        }

        public void StartRecord()
        {
            IsRecording = true;
            _recordTimer.Interval = _noiseDetectionOptions.RecordDuration.TotalMilliseconds;
            _recordTimer.Start();
            var timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            Directory.CreateDirectory(_noiseDetectionOptions.DestinationDirectory);
            var path = Path.Combine(_noiseDetectionOptions.DestinationDirectory, $"autoRecord_{timeStamp}.wav");
            _writer = null;
            _writer = new WaveFileWriter(path, _audioIn.WaveFormat);
            OnAudioEventAvailable(new AudioEventArgs { State = AudioRecordState.RecordStarted, Information = $"SampleRate: {_audioIn.WaveFormat.SampleRate}, BitsPerSample: {_audioIn.WaveFormat.BitsPerSample}, Channels: {_audioIn.WaveFormat.Channels}" });
        }

        private void AudioInRecordingStopped(object s, StoppedEventArgs a)
        {
            _writer?.Dispose();
            _writer = null;
            OnAudioEventAvailable(new AudioEventArgs { State = AudioRecordState.RecordStopped, Information = "Stopped Record." });
            if (!_isStopAndDisposeRequested)
            {
                _audioIn.StartRecording();
            }
            else DisposeAudioDevice();
        }

        private void AudioInDataAvailable(object s, WaveInEventArgs a)
        {
            if (IsRecording)
            {
                _writer.Write(a.Buffer, 0, a.BytesRecorded);
            }

            PeakValue = 0;
            var buffer = new WaveBuffer(a.Buffer);
            // interpret as 32 bit floating point audio
            for (int index = 0; index < a.BytesRecorded / 4; index++)
            {
                var sample = buffer.FloatBuffer[index];

                // absolute value 
                if (sample < 0) sample = -sample;
                // is this the max value?
                if (sample > PeakValue) PeakValue = sample;
            }
            OnAudioEventAvailable(new AudioEventArgs { State = AudioRecordState.SampleAvailable });
            OnTresholdReached();
        }

        public void StopRecord()
        {
            _audioIn.StopRecording();
            _recordTimer.Stop();
            IsRecording = false;
        }


        private void OnTresholdReached()
        {
            if (!IsRecording && PeakValue >= _noiseDetectionOptions.Treshold)
            {
                StartRecord();
            }
            else if (IsRecording && PeakValue >= _noiseDetectionOptions.Treshold && _noiseDetectionOptions.ContinueRecordWhenOverTreshold)
            {
                // reset timer in case peak value has been reached again, while record is running
                _recordTimer.Stop();
                _recordTimer.Start();
            }
        }

        public void StopListen()
        {
            _isStopAndDisposeRequested = true;
            if (IsRecording)
            {
                StopRecord();
            }
            else DisposeAudioDevice();
        }

        private void DisposeAudioDevice()
        {
            _audioIn.Dispose();
            IsListening = false;
            OnAudioEventAvailable(new AudioEventArgs { State = AudioRecordState.ListeningStopped, Information = "Stopped Listening." });
        }
    }
}
