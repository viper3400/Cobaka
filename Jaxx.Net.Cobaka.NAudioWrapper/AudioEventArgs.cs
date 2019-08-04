using System;
using System.Collections.Generic;
using System.Text;

namespace Jaxx.Net.Cobaka.NAudioWrapper
{
    public class AudioEventArgs : EventArgs
    {
        public AudioRecordState State { get; set; }
        public string Information { get; set; }
    }
}
