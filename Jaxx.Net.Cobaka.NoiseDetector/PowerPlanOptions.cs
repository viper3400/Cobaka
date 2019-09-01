using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaxx.Net.Cobaka.NoiseDetector
{
    public class PowerPlanOptions : IPowerPlanOptions
    {
        public bool ChangePowerPlanOnListeningModeChange { get; set; }
        public Guid DesiredPowerPlanWhenNotListening { get; set; }
        public Guid DesiredPowerPlanWhenListening { get; set; }
    }
}
