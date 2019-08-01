﻿using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace Jaxx.Net.Cobaka.NAudioWrapper
{
    public class NAudioHandler
    {
        private WaveFileWriter _writer;
        private WasapiCapture _audioIn;
        private readonly INoiseDetectorOptions _noiseDetectionOptions;
        private readonly Timer _recordTimer;
        public NAudioHandler(INoiseDetectorOptions options)
        {
            _noiseDetectionOptions = options;
            _audioIn = new WasapiCapture();
            _audioIn.DataAvailable += AudioInDataAvailable;
            _audioIn.RecordingStopped += AudioInRecordingStopped;
            PeakValue = 0;
            _audioIn.StartRecording();
            _recordTimer = new Timer { AutoReset = true };
            _recordTimer.Elapsed += RecordTimer_Elapsed;
        }

        private void RecordTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            StopRecord();
        }

        public event EventHandler RecordStarted;
        public event EventHandler SampleAvailable;
        public event EventHandler RecordStopped;

        protected virtual void OnRecordStarted(EventArgs e)
        {
            EventHandler handler = RecordStarted;
            handler?.Invoke(this, e);
        }

        protected virtual void OnSampleAvailable(EventArgs e)
        {
            EventHandler handler = SampleAvailable;
            handler?.Invoke(this, e);
        }
        protected virtual void OnRecordStopped(EventArgs e)
        {
            EventHandler handler = RecordStopped;
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
            var now = DateTime.Now;
            var timeStamp = $"{now.Year}_{now.Month}_{now.Day}_{now.Hour}_{now.Minute}_{now.Second}";
            var path = Path.Combine(_noiseDetectionOptions.DestinationDirectory, $"autoRecord_{timeStamp}.wav");
            _writer = null;
            _writer = new WaveFileWriter(path, new WaveFormat(_audioIn.WaveFormat.SampleRate, _audioIn.WaveFormat.BitsPerSample, _audioIn.WaveFormat.Channels));
            OnRecordStarted(new EventArgs());
        }

        private void AudioInRecordingStopped(object s, StoppedEventArgs a)
        {
            _writer?.Dispose();
            _writer = null;
            //_audioIn.Dispose(); 
            OnRecordStopped(new EventArgs());
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
            OnSampleAvailable(new EventArgs());
            OnTresholdReached();
        }

        public void StopRecord()
        {
            _audioIn.StopRecording();
            IsRecording = false;
        }


        private void OnTresholdReached()
        {
            if (!IsRecording && PeakValue >= _noiseDetectionOptions.Treshold)
            {
                StartRecord();
            }
        }
    }
}
