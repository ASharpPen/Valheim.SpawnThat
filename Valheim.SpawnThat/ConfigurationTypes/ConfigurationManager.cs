﻿using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Valheim.SpawnThat.ConfigurationCore;

namespace Valheim.SpawnThat.ConfigurationTypes
{
    public static class ConfigurationManager
    {
        private const string GeneralConfigFile = "spawn_that.cfg";

        private const string SimpleConfigFile = "spawn_that.simple.cfg";

        private const string SpawnSystemConfigFile = "spawn_that.world_spawners_advanced.cfg";

        private const string CreatureSpawnerConfigFile = "spawn_that.local_spawners_advanced.cfg";

        public static bool DebugOn => true;

        public static GeneralConfig GeneralConfig;

        public static List<SimpleConfig> SimpleConfig;

        public static SpawnSystemConfigurationAdvanced SpawnSystemConfig;

        public static Dictionary<string, CreatureSpawnerConfigurationAdvanced> CreatureSpawnerConfig;

        public static void LoadAllConfigurations()
        {
            Log.LogInfo("Loading all configs.");

            GeneralConfig = LoadGeneral();

            SimpleConfig = LoadSimpleConfig();

            SpawnSystemConfig = LoadSpawnSystemConfigurationAdvanced();

            CreatureSpawnerConfig = LoadCreatureSpawnerConfigurationAdvanced();
        }

        public static GeneralConfig LoadGeneral()
        {
            Log.LogInfo("Loading general configurations");

            string configPath = Path.Combine(Paths.ConfigPath, GeneralConfigFile);
            ConfigFile configFile = new ConfigFile(configPath, true);

            GeneralConfig = new GeneralConfig();
            GeneralConfig.Load(configFile);

            Log.LogInfo("Finished loading general configurations");

            return GeneralConfig;
        }

        public static List<SimpleConfig> LoadSimpleConfig()
        {
            string configPath = Path.Combine(Paths.ConfigPath, SimpleConfigFile);

            Log.LogInfo($"Loading simple spawn configurations from {configPath}.");

            var configFile = new ConfigFile(configPath, true);
            if (GeneralConfig.StopTouchingMyConfigs.Value) configFile.SaveOnConfigSet = false;

            Dictionary<string, SimpleConfig> configurations = ConfigurationLoader.LoadConfigurationGroup<SimpleConfig, SimpleConfigSection>(configFile);

            Log.LogInfo($"Finished loading simple spawn configurations.");

            return configurations.Values.ToList();
        }

        public static SpawnSystemConfigurationAdvanced LoadSpawnSystemConfigurationAdvanced()
        {
            string configPath = Path.Combine(Paths.ConfigPath, SpawnSystemConfigFile);

            Log.LogInfo($"Loading advanced spawn system configurations from {configPath}.");

            var configFile = new ConfigFile(configPath, true);
            if (GeneralConfig.StopTouchingMyConfigs.Value) configFile.SaveOnConfigSet = false;

            Dictionary<string, SpawnSystemConfigurationAdvanced> configurations = ConfigurationLoader.LoadConfigurationGroup<SpawnSystemConfigurationAdvanced, SpawnConfiguration>(configFile);

            Log.LogInfo($"Finished loading advanced spawn system configurations.");

            return configurations?.Values?.FirstOrDefault() ?? new SpawnSystemConfigurationAdvanced();
        }

        public static Dictionary<string, CreatureSpawnerConfigurationAdvanced> LoadCreatureSpawnerConfigurationAdvanced()
        {
            string configPath = Path.Combine(Paths.ConfigPath, CreatureSpawnerConfigFile);

            Log.LogInfo($"Loading advanced creature spawner configurations from {configPath}.");

            var configFile = new ConfigFile(configPath, true);
            if (GeneralConfig.StopTouchingMyConfigs.Value) configFile.SaveOnConfigSet = false;

            Dictionary<string, CreatureSpawnerConfigurationAdvanced> configurations = ConfigurationLoader.LoadConfigurationGroup<CreatureSpawnerConfigurationAdvanced, CreatureSpawnerConfig>(configFile);

            Log.LogInfo($"Finished loading advanced creature spawner configurations.");

            return configurations;
        }
    }
}
