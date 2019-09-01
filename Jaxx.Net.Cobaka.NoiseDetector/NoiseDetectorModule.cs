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
            regionManager.RegisterViewWithRegion("WindowsPowerPlanSettingsRegion", typeof(WindowsPowerPlanSettings));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(NoiseDetectorModuleMainView));            
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<INoiseDetectorOptions, NoiseDetectorOptions>();
            containerRegistry.RegisterSingleton<IPowerPlanOptions, PowerPlanOptions>();
            containerRegistry.RegisterSingleton<IConfigurationProvider, ConfigurationProvider>();
            containerRegistry.RegisterSingleton<IAudioHandler, NAudioHandler>();
        }
    }
}