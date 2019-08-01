using Jaxx.Net.Cobaka.NoiseDetector.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace Jaxx.Net.Cobaka.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IContainerExtension _container;
        IRegionManager _regionManager;
        IEventAggregator _eventAggregator;

        public MainWindow(IContainerExtension container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _eventAggregator.GetEvent<NoiseDetector.StopRequestEvent>().Publish("Closing");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var view = _container.Resolve<NoiseDetectorView>();
            IRegion region = _regionManager.Regions["ContentRegion"];            
            region.Add(view);
        }
    }
}
