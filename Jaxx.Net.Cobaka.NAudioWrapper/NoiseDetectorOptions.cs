using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaxx.Net.Cobaka.NAudioWrapper
{
    public class NoiseDetectorOptions : INoiseDetectorOptions
    {
        public double Treshold { get; set; }
        public TimeSpan RecordDuration { get; set; }
        public string DestinationDirectory { get; set; }
        public bool ContinueRecordWhenOverTreshold { get; set; }
        public bool ListenOnStartup { get; set; }
    }
}
