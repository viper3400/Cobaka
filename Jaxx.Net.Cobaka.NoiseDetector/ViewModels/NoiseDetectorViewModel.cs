using Jaxx.Net.Cobaka.NAudioWrapper;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaxx.Net.Cobaka.NoiseDetector.ViewModels
{
    public class NoiseDetectorViewModel : BindableBase
    {
        private readonly NAudioHandler _audio;
        private INoiseDetectorOptions _noiseDetectorOptions;
        public NoiseDetectorViewModel(INoiseDetectorOptions options)
        {
            _noiseDetectorOptions = options;
            // inital fill settings with default value
            RecordTreshold = _noiseDetectorOptions.Treshold;
            DestinationDirectory = _noiseDetectorOptions.DestinationDirectory;
            DurationInSeconds = (int)_noiseDetectorOptions.RecordDuration.TotalSeconds;

            _audio = new NAudioHandler(_noiseDetectorOptions);
            _audio.RecordStarted += Audio_RecordStarted;
            _audio.RecordStopped += Audio_RecordStopped;
            _audio.SampleAvailable += Audio_SampleAvailable;
        }

        private void Audio_RecordStarted(object sender, EventArgs e)
        {
            StopRecord.RaiseCanExecuteChanged();
            StartRecord.RaiseCanExecuteChanged();
        }

        private void Audio_SampleAvailable(object sender, EventArgs e)
        {
            RaisePropertyChanged("PeakValue");
        }

        private void Audio_RecordStopped(object sender, EventArgs e)
        {
            StopRecord.RaiseCanExecuteChanged();
            StartRecord.RaiseCanExecuteChanged();
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private double _recordTreshold;
        public double RecordTreshold
        {
            get { return _recordTreshold; }
            set
            {                
                SetProperty(ref _recordTreshold, value);
                _noiseDetectorOptions.Treshold = _recordTreshold;
            }
        }

        private int _durationInSeconds;
        public int DurationInSeconds
        {
            get { return _durationInSeconds; }
            set
            {
                SetProperty(ref _durationInSeconds, value);
                _noiseDetectorOptions.RecordDuration = new TimeSpan(0, 0, _durationInSeconds);
            }
        }

        private string _destinationDirectory;
        public string DestinationDirectory
        {
            get { return _destinationDirectory; }
            set
            {
                SetProperty(ref _destinationDirectory, value);
                _noiseDetectorOptions.DestinationDirectory = _destinationDirectory;
            }
        }

        public float PeakValue
        {
            get { return (float)Math.Round(_audio.PeakValue * 100, 2); }
        }

        private IEnumerable<string> deviceList;
        public IEnumerable<string> DeviceList
        {
            get { return deviceList; }
            set { SetProperty(ref deviceList, value); }
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
