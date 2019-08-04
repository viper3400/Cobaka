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
        private IAudioConfigurationProvider _optionsProvider;
        public NoiseDetectorSettingsViewModel(IAudioConfigurationProvider optionsProvider)
        {
            _optionsProvider = optionsProvider;
            // inital fill settings with default value
            LoadOptions();
        }

        private void LoadOptions()
        {
            RecordTreshold = Convert.ToInt32(_optionsProvider.NoiseDetectorOptions.Treshold * 100);
            DestinationDirectory = _optionsProvider.NoiseDetectorOptions.DestinationDirectory;
            DurationInSeconds = (int)_optionsProvider.NoiseDetectorOptions.RecordDuration.TotalSeconds;
            ContinueRecordWhenOverTreshold = _optionsProvider.NoiseDetectorOptions.ContinueRecordWhenOverTreshold;
            ListenOnStartup = _optionsProvider.NoiseDetectorOptions.ListenOnStartup;
        }

        private int _recordTreshold;
        public int RecordTreshold
        {
            get { return _recordTreshold; }
            set
            {
                SetProperty(ref _recordTreshold, value);
                _optionsProvider.NoiseDetectorOptions.Treshold = Convert.ToDouble(_recordTreshold) / 100;
                _optionsProvider.Save();
            }
        }

        private int _durationInSeconds;
        public int DurationInSeconds
        {
            get { return _durationInSeconds; }
            set
            {
                SetProperty(ref _durationInSeconds, value);
                _optionsProvider.NoiseDetectorOptions.RecordDuration = new TimeSpan(0, 0, _durationInSeconds);
                _optionsProvider.Save();
            }
        }

        private string _destinationDirectory;
        public string DestinationDirectory
        {
            get { return _destinationDirectory; }
            set
            {
                SetProperty(ref _destinationDirectory, value);
                _optionsProvider.NoiseDetectorOptions.DestinationDirectory = _destinationDirectory;
                _optionsProvider.Save();
            }
        }
       

        private bool _continueRecordWhenOverTreshold;
        public bool ContinueRecordWhenOverTreshold
        {
            get { return _continueRecordWhenOverTreshold; }
            set
            {
                SetProperty(ref _continueRecordWhenOverTreshold, value);
                _optionsProvider.NoiseDetectorOptions.ContinueRecordWhenOverTreshold = _continueRecordWhenOverTreshold;
                _optionsProvider.Save();
            }
        }

        private bool _listenOnStartup;
        public bool ListenOnStartup
        {
            get { return _listenOnStartup; }
            set { SetProperty(ref _listenOnStartup, value); }
        }

        private DelegateCommand _selectFolderDialog;
        public DelegateCommand SelectFolderDialogCommad =>
            _selectFolderDialog ?? (_selectFolderDialog = new DelegateCommand(ExecuteSelectFolderDialogCommad));

        void ExecuteSelectFolderDialogCommad()
        {
            var openFolderDialog = new FolderBrowserDialog();
            openFolderDialog.SelectedPath = _optionsProvider.NoiseDetectorOptions.DestinationDirectory;
            var result = openFolderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                DestinationDirectory = openFolderDialog.SelectedPath;
                _optionsProvider.Save();
            }
        }

        private DelegateCommand _resetDefaults;
        public DelegateCommand ResetDefaults =>
            _resetDefaults ?? (_resetDefaults = new DelegateCommand(ExecuteResetDefaults));

        void ExecuteResetDefaults()
        {
            // Reset and reload options
            _optionsProvider.Reset();
            LoadOptions();
        }
    }
}
