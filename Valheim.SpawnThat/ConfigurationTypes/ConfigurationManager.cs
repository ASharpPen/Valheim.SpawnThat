using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Valheim.SpawnThat.ConfigurationCore;

namespace Valheim.SpawnThat.ConfigurationTypes
{
    public static class ConfigurationManager
    {
        public const string DefaultDropFile = "spawn_that.cfg";

        public static bool DebugOn => true;

        public static List<SpawnerConfiguration> DropConfigs = null;

        public static void LoadAllConfigurations()
        {
            LoadAllDropTableConfigurations();
        }

        public static void LoadAllDropTableConfigurations()
        {
            string configPath = Path.Combine(Paths.ConfigPath, DefaultDropFile);

            var configs = LoadDropTableConfig(configPath);

            DropConfigs = configs;

            Log.LogDebug("Finished loading drop configurations");
        }

        private static List<SpawnerConfiguration> LoadDropTableConfig(string configPath)
        {
            Log.LogDebug($"Loading drop table configurations from {configPath}.");

            var configFile = new ConfigFile(configPath, true);

            Dictionary<string, SpawnerConfiguration> configurations = ConfigurationLoader.LoadConfigurationGroup<SpawnerConfiguration, SpawnConfiguration>(configFile);

            return configurations.Values.ToList();
        }
    }
}
