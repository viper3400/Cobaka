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
            _audio.RecordStarted += Audio_RecordStarted;
            _audio.RecordStopped += Audio_RecordStopped;
            _audio.SampleAvailable += Audio_SampleAvailable;
            eventAggregator.GetEvent<StopRequestEvent>().Subscribe(OnStopRequested);
        }

        private void OnStopRequested(string obj)
        {
            _audio.StopAndDispose();
        }

        private void Audio_RecordStarted(object sender, AudioEventArgs e)
        {
            DeviceList = new List<string> { e.Information };
            StopRecord.RaiseCanExecuteChanged();
            StartRecord.RaiseCanExecuteChanged();
        }

        private void Audio_SampleAvailable(object sender, EventArgs e)
        {
            RaisePropertyChanged("PeakValue");
            RaisePropertyChanged("PeakBarColor");
        }

        private void Audio_RecordStopped(object sender, EventArgs e)
        {
            StopRecord.RaiseCanExecuteChanged();
            StartRecord.RaiseCanExecuteChanged();
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
        public DelegateCommand StartRecord =>
            _recordAudio ?? (_recordAudio = new DelegateCommand(ExecuteStartRecord, CanStartRecord));

        void ExecuteStartRecord()
        {
            _audio.StartRecord();
            StopRecord.RaiseCanExecuteChanged();
            StartRecord.RaiseCanExecuteChanged();
        }

        bool CanStartRecord()
        {
            return !_audio.IsRecording;
        }

        private DelegateCommand _stopRecordCommand;
        public DelegateCommand StopRecord =>
            _stopRecordCommand ?? (_stopRecordCommand = new DelegateCommand(ExecuteStopRecord, CanExecuteStopRecord));

        void ExecuteStopRecord()
        {
            _audio.StopRecord();
        }

        bool CanExecuteStopRecord()
        {
            return _audio.IsRecording;
        }

    }
}
