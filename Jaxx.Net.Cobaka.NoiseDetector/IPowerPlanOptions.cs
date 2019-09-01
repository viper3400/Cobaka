using System;

namespace Jaxx.Net.Cobaka.NoiseDetector
{
    public interface IPowerPlanOptions
    {
        bool ChangePowerPlanOnListeningModeChange { get; set; }
        Guid DesiredPowerPlanWhenListening { get; set; }
        Guid DesiredPowerPlanWhenNotListening { get; set; }
    }
}