using BepInEx;
using BepInEx.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Core.Configuration;

namespace SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;

public static class CreatureSpawnerConfigurationManager
{
    public static CreatureSpawnerConfigurationFile CreatureSpawnerConfig;

    internal const string CreatureSpawnerConfigFile = "spawn_that.local_spawners_advanced.cfg";

    internal const string CreatureSpawnerSupplemental = "spawn_that.local_spawners.*";

    public static void LoadAllConfigurations()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        CreatureSpawnerConfig = LoadCreatureSpawnerConfiguration();

        stopwatch.Stop();

        Log.LogInfo("Config loading took: " + stopwatch.Elapsed);
        if (stopwatch.Elapsed > TimeSpan.FromSeconds(5)
            && !ConfigurationManager.GeneralConfig.StopTouchingMyConfigs.Value)
        {
            Log.LogInfo("Long loading time detected. Consider setting \"StopTouchingMyConfigs=true\" in spawn_that.cfg to improve loading speed.");
        }
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

        ConfigurationLoader.SanitizeSectionHeaders(configPath);
        var configFile = new ConfigFile(configPath, true);

        if (ConfigurationManager.GeneralConfig?.StopTouchingMyConfigs?.Value == true)
        {
            configFile.SaveOnConfigSet = !ConfigurationManager.GeneralConfig.StopTouchingMyConfigs.Value;
        }

        return ConfigurationLoader.LoadConfiguration<CreatureSpawnerConfigurationFile>(configFile);
    }
}
