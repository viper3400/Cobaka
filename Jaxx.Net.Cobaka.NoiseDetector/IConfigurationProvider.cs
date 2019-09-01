using Jaxx.Net.Cobaka.NAudioWrapper;

namespace Jaxx.Net.Cobaka.NoiseDetector
{
    public interface IConfigurationProvider
    {
        INoiseDetectorOptions NoiseDetectorOptions { get; }
        IPowerPlanOptions PowerPlanOptions { get; }
        void Save();
        void Reset();
    }
}