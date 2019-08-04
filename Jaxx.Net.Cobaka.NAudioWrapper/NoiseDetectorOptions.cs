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
        public NoiseDetectorOptions()
        {
            //// init with default values
            //Treshold = 0.35;
            //RecordDuration = new System.TimeSpan(0, 0, 10);
            //DestinationDirectory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Cobaka", "NoiseDetectorRecords");
            //ContinueRecordWhenOverTreshold = true;
        }
        public double Treshold { get; set; }
        public TimeSpan RecordDuration { get; set; }
        public string DestinationDirectory { get; set; }
        public bool ContinueRecordWhenOverTreshold { get; set; }
    }
}
