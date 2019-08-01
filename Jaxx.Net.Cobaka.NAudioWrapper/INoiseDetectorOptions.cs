using System;

namespace Jaxx.Net.Cobaka.NAudioWrapper
{
    public interface INoiseDetectorOptions
    {
        TimeSpan RecordDuration { get; set; }
        double Treshold { get; set; }
        string DestinationDirectory { get; set; }
    }
}