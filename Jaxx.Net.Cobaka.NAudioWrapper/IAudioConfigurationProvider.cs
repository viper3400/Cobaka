namespace Jaxx.Net.Cobaka.NAudioWrapper
{
    public interface IAudioConfigurationProvider
    {
        INoiseDetectorOptions NoiseDetectorOptions { get; }
        void Save();
        void Reset();
    }
}