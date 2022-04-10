﻿using System;
using System.Diagnostics;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Core.Configuration;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration.BepInEx;

internal static class DestructibleSpawnerBepInExCfgManager
{
    private const string ConfigFile = "spawn_that.destructible.cfg";
    private const string ConfigFileSupplemental = "spawn_that.destructible.*.cfg";

    internal static DestructibleSpawnerConfigurationFile Config { get; private set; }

    public static void Load()
    {
        try
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Log.LogInfo($"Loading destructible spawner configurations.");

            string configPath = Path.Combine(Paths.ConfigPath, ConfigFile);

            var configs = LoadConfig(configPath);

            var supplementalFiles = Directory.GetFiles(Paths.ConfigPath, ConfigFileSupplemental, SearchOption.AllDirectories);
            Log.LogDebug($"Found {supplementalFiles.Length} supplemental destructible spawner config files");

            foreach (var file in supplementalFiles)
            {
                try
                {
                    var supplementalConfig = LoadConfig(file);

                    supplementalConfig.MergeInto(configs);
                }
                catch (Exception e)
                {
                    Log.LogError($"Failed to load supplemental config '{file}'.", e);
                }
            }

            stopwatch.Stop();

            Log.LogInfo("Config loading took: " + stopwatch.Elapsed);
            if (stopwatch.Elapsed > TimeSpan.FromSeconds(5) &&
                ConfigurationManager.GeneralConfig?.StopTouchingMyConfigs?.Value == false)
            {
                Log.LogInfo("Long loading time detected. Consider setting \"StopTouchingMyConfigs=true\" in spawn_that.cfg to improve loading speed.");
            }

            Config = configs;
        }
        catch (Exception e)
        {
            Log.LogError("Error during loading of destructible spawner cfg files.", e);
        }

        static DestructibleSpawnerConfigurationFile LoadConfig(string configPath)
        {
            Log.LogDebug($"Loading destructible spawner configurations from {configPath}.");

            ConfigurationLoader.SanitizeSectionHeaders(configPath);
            var configFile = new ConfigFile(configPath, true);

            if (ConfigurationManager.GeneralConfig?.StopTouchingMyConfigs?.Value == true)
            {
                configFile.SaveOnConfigSet = !ConfigurationManager.GeneralConfig.StopTouchingMyConfigs.Value;
            }

            return ConfigurationLoader.LoadConfiguration<DestructibleSpawnerConfigurationFile>(configFile);
        }
    }
}