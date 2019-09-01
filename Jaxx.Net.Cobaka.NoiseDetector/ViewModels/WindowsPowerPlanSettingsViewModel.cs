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
        public WindowsPowerPlanSettingsViewModel(IEventAggregator eventAggregator)
        {            
            DesiredPowerPlanWhenNotListening = PowerPlan.GetActive();
            AvailablePowerPlans = PowerPlan.GetAvailablePowerPlans();
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<NoiseDetectorChangeEvent>().Subscribe(OnNoiseDetectorChangeEvent);
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
            set { SetProperty(ref _isChangePPWhenListeningModeChanged, value); }
        }

        private Dictionary<Guid,string> _availablePowerPlans;
        public Dictionary<Guid,string> AvailablePowerPlans
        {
            get { return _availablePowerPlans; }
            set { SetProperty(ref _availablePowerPlans, value); }
        }

        private Guid _DesiredPowerPlanWhenNotListening;
        public Guid DesiredPowerPlanWhenNotListening
        {
            get { return _DesiredPowerPlanWhenNotListening; }
            set
            {
                SetProperty(ref _DesiredPowerPlanWhenNotListening, value);
            }
        }

        private Guid _desiredPowerPlanWhenListening;
        public Guid DesiredPowerPlanWhenListening
        {
            get { return _desiredPowerPlanWhenListening; }
            set { SetProperty(ref _desiredPowerPlanWhenListening, value); }
        }
    }
}
