using System;
using System.Collections.Generic;

namespace Jaxx.Net.Cobaka.NAudioWrapper
{
    public interface IAudioHandler
    {
        List<string> DeviceList { get; }
        bool IsRecording { get; }
        float PeakValue { get; }

        event EventHandler RecordStarted;
        event EventHandler RecordStopped;
        event EventHandler SampleAvailable;

        void StartRecord();
        void StopAndDispose();
        void StopRecord();
    }
}