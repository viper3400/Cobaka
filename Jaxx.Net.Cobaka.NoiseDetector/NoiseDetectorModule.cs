using Jaxx.Net.Cobaka.NAudioWrapper;
using Jaxx.Net.Cobaka.NoiseDetector.ViewModels;
using Jaxx.Net.Cobaka.NoiseDetector.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.IO;

namespace Jaxx.Net.Cobaka.NoiseDetector
{
    public class NoiseDetectorModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("NoiseDetectorModuleMainRegion", typeof(NoiseDetectorView));
            regionManager.RegisterViewWithRegion("NoiseDetectorModuleSettingsRegion", typeof(NoiseDetectorSettingsView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(NoiseDetectorModuleMainView));            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<INoiseDetectorOptions>(new NoiseDetectorOptions { Treshold = 0.35, RecordDuration = new System.TimeSpan(0, 0, 10), DestinationDirectory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),"Cobaka","NoiseDetectorRecords"), ContinueRecordWhenOverTreshold = true });
            containerRegistry.Register<IAudioHandler, NAudioHandler>();
        }
    }
}