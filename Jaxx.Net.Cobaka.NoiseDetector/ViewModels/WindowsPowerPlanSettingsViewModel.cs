using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Jaxx.Net.Cobaka.WinPowerPlan;

namespace Jaxx.Net.Cobaka.NoiseDetector.ViewModels
{
    public class WindowsPowerPlanSettingsViewModel : BindableBase
    {
        public WindowsPowerPlanSettingsViewModel()
        {
            ActivePowerPlan = PowerPlan.GetActive();
            AvailablePowerPlans = PowerPlan.GetAvailablePowerPlans();
        }

        private Dictionary<Guid,string> _availablePowerPlans;
        public Dictionary<Guid,string> AvailablePowerPlans
        {
            get { return _availablePowerPlans; }
            set { SetProperty(ref _availablePowerPlans, value); }
        }

        private Guid _activePowerPlan;
        public Guid ActivePowerPlan
        {
            get { return _activePowerPlan; }
            set
            {
                SetProperty(ref _activePowerPlan, value);
                PowerPlan.SetActive(ActivePowerPlan);
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
