using System;
using System.Collections.Generic;

namespace Jaxx.Net.Cobaka.NAudioWrapper
{
    public interface IAudioHandler
    {
        List<string> DeviceList { get; }
        bool IsRecording { get; }
        bool IsListening { get; }
        float PeakValue { get; }

        event EventHandler<AudioEventArgs> AudioEventAvailable;
        void StartListen();
        void StartRecord();
        void StopListen();
        void StopRecord();
    }
}