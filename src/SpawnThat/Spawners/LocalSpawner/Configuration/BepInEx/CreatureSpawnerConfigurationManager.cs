using BepInEx;
using BepInEx.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Core.Configuration;
using System.Linq;
using SpawnThat.Core.Toml;

namespace SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;

internal static class CreatureSpawnerConfigurationManager
{
    public static CreatureSpawnerConfigurationFile CreatureSpawnerConfig;

    internal const string CreatureSpawnerConfigFile = "spawn_that.local_spawners_advanced.cfg";

    internal const string CreatureSpawnerSupplemental = "spawn_that.local_spawners.*";

    public static void LoadAllConfigurations()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        CreatureSpawnerConfig = LoadCreatureSpawnerConfiguration();

        stopwatch.Stop();

        Log.LogDebug("Config loading took: " + stopwatch.Elapsed);
    }

    public static CreatureSpawnerConfigurationFile LoadCreatureSpawnerConfiguration()
    {
        Log.LogInfo($"Loading local spawner configurations.");

        string configPath = Path.Combine(Paths.ConfigPath, CreatureSpawnerConfigFile);

        if (!File.Exists(configPath))
        {
            CreateDefaultLocalSpawnerFile(configPath);
        }

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

        return TomlLoader.LoadFile<CreatureSpawnerConfigurationFile>(configPath);
    }

    private static void CreateDefaultLocalSpawnerFile(string configPath)
    {
        using var file = File.Create(configPath);
        using var writer = new StreamWriter(file);

        writer.WriteLine("# Auto-generated file for adding Local Spawner configurations.");
        writer.WriteLine("# This file is empty by default. It is intended to contains changes only, to avoid unintentional modifications as well as to reduce unnecessary performance cost.");
        writer.WriteLine("# Full documentation can be found at https://asharppen.github.io/Valheim.SpawnThat.");
        writer.WriteLine("# To get started: ");
        writer.WriteLine($"#     1. Generate default configs in BepInEx/Debug folder, by enabling WriteSpawnTablesToFileBeforeChanges in 'spawn_that.cfg'.");
        writer.WriteLine($"#     2. Start game and enter a world, and wait a short moment (ca. 10 seconds) for files to generate.");
        writer.WriteLine("#     3. Go to generated file, and copy the creatures you want to modify into this file. Multiple local spawner files will have been generated, use either the one for locations or dungeons, depending on what you want to modify.");
        writer.WriteLine("#     4. Make your changes.");
        writer.WriteLine();
    }
}
