using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Jaxx.Net.Cobaka.NAudioWrapper;

namespace Jaxx.Net.Cobaka.NoiseDetector
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly string _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cobaka");
        private readonly string _configFile;
        public ConfigurationProvider(INoiseDetectorOptions noiseDetectorOptions, IPowerPlanOptions ppOptions)
        {            
            // init with default values
            NoiseDetectorOptions = noiseDetectorOptions;
            PowerPlanOptions = ppOptions;
            InitDefaults();

            // check for existing config
            _configFile = Path.Combine(_configPath, "NoiseDetectorConfig.json");
            Load();
        }
        public void Save()
        {
            Directory.CreateDirectory(_configPath);
            using (StreamWriter file = File.CreateText(_configFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                var configObject = new Dictionary<string, object>();
                configObject.Add(NoiseDetectorOptions.GetType().Name, NoiseDetectorOptions);
                configObject.Add(PowerPlanOptions.GetType().Name, PowerPlanOptions);
                //serialize object directly into file stream
                serializer.Serialize(file, configObject);
            }
        }

        private void Load()
        {            
            if (File.Exists(_configFile))
            {
                var jsonString = File.ReadAllText(_configFile);
                var jsonObject = JObject.Parse(jsonString);
                NoiseDetectorOptions.Treshold = (double)jsonObject["NoiseDetectorOptions"]["Treshold"];
                NoiseDetectorOptions.RecordDuration = (TimeSpan)jsonObject["NoiseDetectorOptions"]["RecordDuration"];
                NoiseDetectorOptions.DestinationDirectory = (string)jsonObject["NoiseDetectorOptions"]["DestinationDirectory"];
                NoiseDetectorOptions.ContinueRecordWhenOverTreshold = (bool)jsonObject["NoiseDetectorOptions"]["ContinueRecordWhenOverTreshold"];
                NoiseDetectorOptions.ListenOnStartup = (bool)jsonObject["NoiseDetectorOptions"]["ListenOnStartup"];
                PowerPlanOptions.ChangePowerPlanOnListeningModeChange = (bool)jsonObject["PowerPlanOptions"]["ChangePowerPlanOnListeningModeChange"];
                PowerPlanOptions.DesiredPowerPlanWhenListening =(Guid)jsonObject["PowerPlanOptions"]["DesiredPowerPlanWhenListening"];
                PowerPlanOptions.DesiredPowerPlanWhenNotListening = (Guid)jsonObject["PowerPlanOptions"]["DesiredPowerPlanWhenNotListening"];
            }
        }

        private void InitDefaults()
        {
            // init with default values
            NoiseDetectorOptions.Treshold = 0.35;
            NoiseDetectorOptions.RecordDuration = new System.TimeSpan(0, 0, 10);
            NoiseDetectorOptions.DestinationDirectory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Cobaka", "NoiseDetectorRecords");
            NoiseDetectorOptions.ContinueRecordWhenOverTreshold = true;
            NoiseDetectorOptions.ListenOnStartup = false;
            PowerPlanOptions.ChangePowerPlanOnListeningModeChange = false;
            PowerPlanOptions.DesiredPowerPlanWhenListening = Guid.Empty;
            PowerPlanOptions.DesiredPowerPlanWhenNotListening = Guid.Empty;
        }

        public void Reset()
        {
            // Delete settings and reload default values
            File.Delete(_configFile);
            InitDefaults();
        }

        public INoiseDetectorOptions NoiseDetectorOptions { get; private set; }
        public IPowerPlanOptions PowerPlanOptions { get; private set; }
    }
}
