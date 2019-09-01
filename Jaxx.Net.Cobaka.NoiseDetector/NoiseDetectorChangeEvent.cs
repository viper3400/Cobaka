using Prism.Events;

namespace Jaxx.Net.Cobaka.NoiseDetector
{
    public class NoiseDetectorChangeEvent : PubSubEvent<NoiseDetectorEvent> { }
    public enum NoiseDetectorEvent { StopRequested, StartListening, StopListening }
}
