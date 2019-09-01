using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Jaxx.Net.Cobaka.WinPowerPlan;
using Prism.Events;

namespace Jaxx.Net.Cobaka.NoiseDetector.ViewModels
{
    public class WindowsPowerPlanSettingsViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private IConfigurationProvider _optionsProvider;
        public WindowsPowerPlanSettingsViewModel(IEventAggregator eventAggregator, IConfigurationProvider optionsProvider)
        {
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<NoiseDetectorChangeEvent>().Subscribe(OnNoiseDetectorChangeEvent);

            _optionsProvider = optionsProvider;
            LoadOptions();
        }

        private void LoadOptions()
        {
            AvailablePowerPlans = PowerPlan.GetAvailablePowerPlans();

            if (AvailablePowerPlans.Count > 0)
            {
                IsChangePPWhenListeningModeChanged = _optionsProvider.PowerPlanOptions.ChangePowerPlanOnListeningModeChange;

                DesiredPowerPlanWhenNotListening = _optionsProvider.PowerPlanOptions.DesiredPowerPlanWhenNotListening == Guid.Empty ? DesiredPowerPlanWhenNotListening = PowerPlan.GetActive() : _optionsProvider.PowerPlanOptions.DesiredPowerPlanWhenNotListening;

                DesiredPowerPlanWhenListening = _optionsProvider.PowerPlanOptions.DesiredPowerPlanWhenListening == Guid.Empty ? DesiredPowerPlanWhenListening = PowerPlan.GetActive() : _optionsProvider.PowerPlanOptions.DesiredPowerPlanWhenListening;
            }
        }

        private void OnNoiseDetectorChangeEvent(NoiseDetectorEvent evnt)
        {
            switch (evnt)
            {
                case NoiseDetectorEvent.StartListening:
                    if (IsChangePPWhenListeningModeChanged) PowerPlan.SetActive(DesiredPowerPlanWhenListening);
                    break;
                case NoiseDetectorEvent.StopListening:
                    if (IsChangePPWhenListeningModeChanged) PowerPlan.SetActive(DesiredPowerPlanWhenNotListening);
                    break;
            }
        }

        private bool _isChangePPWhenListeningModeChanged;
        public bool IsChangePPWhenListeningModeChanged
        {
            get { return _isChangePPWhenListeningModeChanged; }
            set
            {
                SetProperty(ref _isChangePPWhenListeningModeChanged, value);
                _optionsProvider.PowerPlanOptions.ChangePowerPlanOnListeningModeChange = IsChangePPWhenListeningModeChanged;
                _optionsProvider.Save();
            }
        }

        private Dictionary<Guid, string> _availablePowerPlans;
        public Dictionary<Guid, string> AvailablePowerPlans
        {
            get { return _availablePowerPlans; }
            set { SetProperty(ref _availablePowerPlans, value); }
        }

        private Guid _desiredPowerPlanWhenNotListening;
        public Guid DesiredPowerPlanWhenNotListening
        {
            get { return _desiredPowerPlanWhenNotListening; }
            set
            {
                SetProperty(ref _desiredPowerPlanWhenNotListening, value);
                _optionsProvider.PowerPlanOptions.DesiredPowerPlanWhenNotListening = DesiredPowerPlanWhenNotListening;
                _optionsProvider.Save();

            }
        }

        private Guid _desiredPowerPlanWhenListening;
        public Guid DesiredPowerPlanWhenListening
        {
            get { return _desiredPowerPlanWhenListening; }
            set
            {
                SetProperty(ref _desiredPowerPlanWhenListening, value);
                _optionsProvider.PowerPlanOptions.DesiredPowerPlanWhenListening = DesiredPowerPlanWhenListening;
                _optionsProvider.Save();
            }
        }
    }
}
