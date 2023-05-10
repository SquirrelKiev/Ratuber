using Newtonsoft.Json;
using Ratuber.Client.Data.Rules;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Ratuber.Client.Data
{
    [Serializable]
    public class Config
    {
        #region Serializeable Data

        public ObservableCollection<MicrophoneConfig> MicConfigs { get; private set; } = new();

        public ObservableCollection<LayerGroup> LayerGroups { get; private set; } = new();
        #endregion

        public static Config CurrentConfig { get; private set; }

        public static event Action OnConfigUpdated;

        private static string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ratuber");
        private static string configPath = Path.Combine(basePath, "config.json");

        public void Initialize()
        {
            MicConfigs.CollectionChanged += CollectionChanged;
            LayerGroups.CollectionChanged += CollectionChanged;

            OnConfigUpdated += ConfigUpdated;

            OnConfigUpdated.Invoke();
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnConfigUpdated.Invoke();
        }

        public static void ConfigUpdated()
        {
            CurrentState.MicrophoneManagers.Clear();

            foreach (var micConfig in CurrentConfig.MicConfigs)
            {
                CurrentState.MicrophoneManagers.Add(new MicrophoneManager().Initialize(micConfig));
            }

            RuleEvaluator.UpdateInputDictionary(true);

            SaveConfig();
        }

        public static void SaveConfig()
        {
            var configSerialized = JsonConvert.SerializeObject(CurrentConfig);

            File.WriteAllText(configPath, configSerialized);
        }

        public static void LoadConfig()
        {
            Directory.CreateDirectory(basePath);

            CurrentConfig = new Config();

            if (!File.Exists(configPath))
            {
                SaveConfig();
            }

            var configSerialized = File.ReadAllText(configPath);

            JsonConvert.PopulateObject(configSerialized, CurrentConfig);

            CurrentConfig.Initialize();
        }
    }
}
