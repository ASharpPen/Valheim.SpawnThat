using BepInEx;
using BepInEx.Configuration;
using System;
using System.IO;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.PreConfigured;

namespace Valheim.SpawnThat.Configuration
{
    public static class ConfigurationManager
    {
        public static GeneralConfiguration GeneralConfig;
        public static SpawnSystemConfigurationFile SpawnSystemConfig;
        public static CreatureSpawnerConfigurationFile CreatureSpawnerConfig;
        public static SimpleConfigurationFile SimpleConfig;

        internal const string GeneralConfigFile = "spawn_that.cfg";
        internal const string SimpleConfigFile = "spawn_that.simple.cfg";
        internal const string SpawnSystemConfigFile = "spawn_that.world_spawners_advanced.cfg";
        internal const string CreatureSpawnerConfigFile = "spawn_that.local_spawners_advanced.cfg";

        internal const string SpawnSystemSupplemental = "spawn_that.world_spawners.*";
        internal const string CreatureSpawnerSupplemental = "spawn_that.local_spawners.*";

        public static void LoadAllConfigurations()
        {
            Log.LogInfo("Loading all configs.");

            GeneralConfig = LoadGeneral();

            SimpleConfig = LoadSimpleConfig();

            SpawnSystemConfig = LoadSpawnSystemConfiguration();

            CreatureSpawnerConfig = LoadCreatureSpawnerConfiguration();
        }

        public static GeneralConfiguration LoadGeneral()
        {
            Log.LogInfo("Loading general configurations");

            string configPath = Path.Combine(Paths.ConfigPath, GeneralConfigFile);
            ConfigFile configFile = new ConfigFile(configPath, true);

            var generalConfig = new GeneralConfiguration();
            generalConfig.Load(configFile);

            Log.LogInfo("Finished loading general configurations");

            return generalConfig;
        }

        public static SimpleConfigurationFile LoadSimpleConfig()
        {
            Log.LogInfo("Loading simple configurations");

            string configPath = Path.Combine(Paths.ConfigPath, SimpleConfigFile);
            if (!File.Exists(configPath) && GeneralConfig?.InitializeWithCreatures?.Value == true)
            {
                SimpleConfigAllCreatures.Initialize();
            }

            ConfigFile configFile = new ConfigFile(configPath, true);

            if (GeneralConfig?.StopTouchingMyConfigs?.Value == true) configFile.SaveOnConfigSet = !GeneralConfig.StopTouchingMyConfigs.Value;

            var config = ConfigurationLoader.LoadConfiguration<SimpleConfigurationFile>(configFile);
            Log.LogDebug("Finished loading simple configurations");

            return config;
        }

        public static SpawnSystemConfigurationFile LoadSpawnSystemConfiguration()
        {
            Log.LogInfo($"Loading world spawner configurations.");

            string configPath = Path.Combine(Paths.ConfigPath, SpawnSystemConfigFile);

            var configs = LoadSpawnSystemConfig(configPath);

            var supplementalFiles = Directory.GetFiles(Paths.ConfigPath, SpawnSystemSupplemental, SearchOption.AllDirectories);
            Log.LogDebug($"Found {supplementalFiles.Length} supplemental world spawner config files");

            foreach (var file in supplementalFiles)
            {
                try
                {
                    var supplementalConfig = LoadSpawnSystemConfig(file);

                    supplementalConfig.MergeInto(configs);
                }
                catch(Exception e)
                {
                    Log.LogError($"Failed to load supplemental config '{file}'.", e);
                }
            }

            Log.LogDebug("Finished loading world spawner configurations");

            return configs;
        }

        private static SpawnSystemConfigurationFile LoadSpawnSystemConfig(string configPath)
        {
            Log.LogDebug($"Loading world spawner configurations from {configPath}.");

            var configFile = new ConfigFile(configPath, true);

            if (GeneralConfig?.StopTouchingMyConfigs?.Value == true) configFile.SaveOnConfigSet = !GeneralConfig.StopTouchingMyConfigs.Value;

            return ConfigurationLoader.LoadConfiguration<SpawnSystemConfigurationFile>(configFile);
        }

        public static CreatureSpawnerConfigurationFile LoadCreatureSpawnerConfiguration()
        {
            Log.LogInfo($"Loading local spawner configurations.");

            string configPath = Path.Combine(Paths.ConfigPath, CreatureSpawnerConfigFile);

            var configs = LoadCreatureSpawnConfig(configPath);

            var supplementalFiles = Directory.GetFiles(Paths.ConfigPath, CreatureSpawnerSupplemental, SearchOption.AllDirectories);
            Log.LogDebug($"Found {supplementalFiles.Length} supplemental local spawner config files");

            foreach (var file in supplementalFiles)
            {
                try
                {
                    var supplementalConfig = LoadCreatureSpawnConfig(file);

                    supplementalConfig.MergeInto(configs);
                }
                catch (Exception e)
                {
                    Log.LogError($"Failed to load supplemental config '{file}'.", e);
                }
            }

            Log.LogDebug("Finished loading local spawner configurations");

            return configs;
        }

        private static CreatureSpawnerConfigurationFile LoadCreatureSpawnConfig(string configPath)
        {
            Log.LogDebug($"Loading local spawner configurations from {configPath}.");

            var configFile = new ConfigFile(configPath, true);

            if (GeneralConfig?.StopTouchingMyConfigs?.Value == true) configFile.SaveOnConfigSet = !GeneralConfig.StopTouchingMyConfigs.Value;

            return ConfigurationLoader.LoadConfiguration<CreatureSpawnerConfigurationFile>(configFile);
        }
    }
}
