using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jaxx.Net.Cobaka.NAudioWrapper
{
    public class AudioConfigurationProvider : IAudioConfigurationProvider
    {
        private readonly string _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cobaka");
        private readonly string _configFile;
        public AudioConfigurationProvider(INoiseDetectorOptions noiseDetectorOptions)
        {            
            // init with default values
            NoiseDetectorOptions = noiseDetectorOptions;
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
                //serialize object directly into file stream
                serializer.Serialize(file, NoiseDetectorOptions);
            }
        }

        private void Load()
        {            
            if (File.Exists(_configFile))
            {
                var jsonString = File.ReadAllText(_configFile);
                var jsonObject = JObject.Parse(jsonString);
                NoiseDetectorOptions.Treshold = (double)jsonObject["Treshold"];
                NoiseDetectorOptions.RecordDuration = (TimeSpan)jsonObject["RecordDuration"];
                NoiseDetectorOptions.DestinationDirectory = (string)jsonObject["DestinationDirectory"];
                NoiseDetectorOptions.ContinueRecordWhenOverTreshold = (bool)jsonObject["ContinueRecordWhenOverTreshold"];
                NoiseDetectorOptions.ListenOnStartup = (bool)jsonObject["ListenOnStartup"];
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
        }

        public void Reset()
        {
            // Delete settings and reload default values
            File.Delete(_configFile);
            InitDefaults();
        }

        public INoiseDetectorOptions NoiseDetectorOptions { get; private set; }
    }
}
