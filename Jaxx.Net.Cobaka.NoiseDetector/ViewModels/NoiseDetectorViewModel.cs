using Jaxx.Net.Cobaka.NAudioWrapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Jaxx.Net.Cobaka.NoiseDetector.ViewModels
{
    public class NoiseDetectorViewModel : BindableBase
    {
        private readonly IAudioHandler _audio;
        private INoiseDetectorOptions _noiseDetectorOptions;
        public NoiseDetectorViewModel(INoiseDetectorOptions options, IAudioHandler audio, IEventAggregator eventAggregator)
        {
            _noiseDetectorOptions = options;
            _audio = audio;
            _audio.AudioEventAvailable += OnAudioEventAvailable;
            eventAggregator.GetEvent<StopRequestEvent>().Subscribe(OnStopRequested);
        }

        private void OnStopRequested(string obj)
        {
            _audio.StopListen();
        }

        private void OnAudioEventAvailable(object sender, AudioEventArgs e)
        {
            switch (e.State)
            {
                case AudioRecordState.SampleAvailable:
                    RaisePropertyChanged("PeakValue");
                    RaisePropertyChanged("PeakBarColor");
                    break;
                default:
                    DeviceList = new List<string> { e.Information };
                    StopListening.RaiseCanExecuteChanged();
                    StartListening.RaiseCanExecuteChanged();
                    break;
            }
        }

        public float PeakValue
        {
            get { return (float)Math.Round(_audio.PeakValue * 100); }
        }

        private IEnumerable<string> deviceList;
        public IEnumerable<string> DeviceList
        {
            get { return deviceList; }
            set { SetProperty(ref deviceList, value); }
        }
        public Brush PeakBarColor
        {
            get
            {
                var _peakBarColor = Brushes.Green;
                if (_audio.PeakValue >= _noiseDetectorOptions.Treshold) _peakBarColor = Brushes.Red;
                return _peakBarColor;
            }
        }

        private DelegateCommand _deviceList;
        public DelegateCommand GetDeviceList =>
            _deviceList ?? (_deviceList = new DelegateCommand(ExecuteGetDeviceList, CanExecuteGetDeviceList));

        void ExecuteGetDeviceList()
        {
            DeviceList = _audio.DeviceList;
        }

        bool CanExecuteGetDeviceList()
        {
            return true;
        }

        private DelegateCommand _exploreRecords;
        public DelegateCommand ExploreRecords =>
            _exploreRecords ?? (_exploreRecords = new DelegateCommand(ExecuteExploreRecords));

        void ExecuteExploreRecords()
        {
            Process.Start(_noiseDetectorOptions.DestinationDirectory);
        }

        private DelegateCommand _recordAudio;
        public DelegateCommand StartListening =>
            _recordAudio ?? (_recordAudio = new DelegateCommand(ExecuteStartListen, CanExecuteStartListen));

        void ExecuteStartListen()
        {
            _audio.StartListen();
            StopListening.RaiseCanExecuteChanged();
            StartListening.RaiseCanExecuteChanged();
        }

        bool CanExecuteStartListen()
        {
            return !_audio.IsListening;
        }

        private DelegateCommand _stopListening;
        public DelegateCommand StopListening =>
            _stopListening ?? (_stopListening = new DelegateCommand(ExecuteStopListen, CanExecuteStopListen));

        void ExecuteStopListen()
        {
            _audio.StopListen();
        }

        bool CanExecuteStopListen()
        {
            return _audio.IsListening;
        }

    }
}
