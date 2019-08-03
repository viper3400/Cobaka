using Jaxx.Net.Cobaka.NAudioWrapper;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Jaxx.Net.Cobaka.NoiseDetector.ViewModels
{
    public class NoiseDetectorSettingsViewModel : BindableBase
    {
        private INoiseDetectorOptions _noiseDetectorOptions;
        public NoiseDetectorSettingsViewModel(INoiseDetectorOptions options)
        {
            _noiseDetectorOptions = options;
            // inital fill settings with default value
            RecordTreshold = Convert.ToInt32(_noiseDetectorOptions.Treshold * 100);
            DestinationDirectory = _noiseDetectorOptions.DestinationDirectory;
            DurationInSeconds = (int)_noiseDetectorOptions.RecordDuration.TotalSeconds;
            ContinueRecordWhenOverTreshold = _noiseDetectorOptions.ContinueRecordWhenOverTreshold;
        }

        private int _recordTreshold;
        public int RecordTreshold
        {
            get { return _recordTreshold; }
            set
            {
                SetProperty(ref _recordTreshold, value);
                _noiseDetectorOptions.Treshold = Convert.ToDouble(_recordTreshold) / 100;
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
       

        private bool _continueRecordWhenOverTreshold;
        public bool ContinueRecordWhenOverTreshold
        {
            get { return _continueRecordWhenOverTreshold; }
            set
            {
                SetProperty(ref _continueRecordWhenOverTreshold, value);
                _noiseDetectorOptions.ContinueRecordWhenOverTreshold = _continueRecordWhenOverTreshold;
            }
        }

        private DelegateCommand _selectFolderDialog;
        public DelegateCommand SelectFolderDialogCommad =>
            _selectFolderDialog ?? (_selectFolderDialog = new DelegateCommand(ExecuteSelectFolderDialogCommad));

        void ExecuteSelectFolderDialogCommad()
        {
            var openFolderDialog = new FolderBrowserDialog();
            openFolderDialog.SelectedPath = _noiseDetectorOptions.DestinationDirectory;
            var result = openFolderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                DestinationDirectory = openFolderDialog.SelectedPath;
            }
        }
    }
}
