using BepInEx;
using BepInEx.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Core.Configuration;

namespace SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

public static class SpawnSystemConfigurationManager
{
    public static SpawnSystemConfigurationFile SpawnSystemConfig;
    public static SimpleConfigurationFile SimpleConfig;

    internal const string SimpleConfigFile = "spawn_that.simple.cfg";
    internal const string SpawnSystemConfigFile = "spawn_that.world_spawners_advanced.cfg";

    internal const string SpawnSystemSupplemental = "spawn_that.world_spawners.*";

    public static void LoadAllConfigurations()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        SimpleConfig = LoadSimpleConfig();
        SpawnSystemConfig = LoadSpawnSystemConfiguration();

        stopwatch.Stop();

        Log.LogInfo("Config loading took: " + stopwatch.Elapsed);
        if (stopwatch.Elapsed > TimeSpan.FromSeconds(5) &&
            ConfigurationManager.GeneralConfig?.StopTouchingMyConfigs?.Value == false)
        {
            Log.LogInfo("Long loading time detected. Consider setting \"StopTouchingMyConfigs=true\" in spawn_that.cfg to improve loading speed.");
        }
    }

    public static SimpleConfigurationFile LoadSimpleConfig()
    {
        Log.LogInfo("Loading simple configurations");

        string configPath = Path.Combine(Paths.ConfigPath, SimpleConfigFile);

        if (!File.Exists(configPath) && ConfigurationManager.GeneralConfig?.InitializeWithCreatures?.Value == true)
        {
            SimpleConfigPreconfiguration.Initialize();
        }

        ConfigurationLoader.SanitizeSectionHeaders(configPath);
        ConfigFile configFile = new ConfigFile(configPath, true);

        if (ConfigurationManager.GeneralConfig?.StopTouchingMyConfigs?.Value == true)
        {
            configFile.SaveOnConfigSet = !ConfigurationManager.GeneralConfig.StopTouchingMyConfigs.Value;
        }

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
            catch (Exception e)
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

        ConfigurationLoader.SanitizeSectionHeaders(configPath);
        var configFile = new ConfigFile(configPath, true);

        if (ConfigurationManager.GeneralConfig?.StopTouchingMyConfigs?.Value == true)
        {
            configFile.SaveOnConfigSet = !ConfigurationManager.GeneralConfig.StopTouchingMyConfigs.Value;
        }

        return ConfigurationLoader.LoadConfiguration<SpawnSystemConfigurationFile>(configFile);
    }
}
