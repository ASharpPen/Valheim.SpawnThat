using System;
using System.Diagnostics;
using System.IO;
using BepInEx;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Core.Toml;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Configuration.BepInEx;

internal static class SpawnAreaSpawnerTomlCfgManager
{
    private const string ConfigFile = "spawn_that.spawnarea_spawners.cfg";
    private const string ConfigFileSupplemental = "spawn_that.spawnarea_spawners.*.cfg";

    internal static SpawnAreaSpawnerConfigurationFile Config { get; private set; }

    public static void Load()
    {
        try
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Log.LogInfo($"Loading SpawnArea spawner configurations.");

            string configPath = Path.Combine(Paths.ConfigPath, ConfigFile);

            if (!File.Exists(configPath))
            {
                CreateSpawnAreaSpawnerFile(configPath);
            }

            SpawnAreaSpawnerConfigurationFile configs = new();

            var supplementalFiles = Directory.GetFiles(Paths.ConfigPath, ConfigFileSupplemental, SearchOption.AllDirectories);
            Log.LogDebug($"Found {supplementalFiles.Length} supplemental SpawnArea spawner config files");

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

            var mainConfig = LoadConfig(configPath);
            mainConfig.MergeInto(configs);

            stopwatch.Stop();

            Log.LogInfo("Loading SpawnArea spawner configs took: " + stopwatch.Elapsed);

            Config = configs;
        }
        catch (Exception e)
        {
            Log.LogError("Error during loading of SpawnArea spawner cfg files.", e);
        }

        static SpawnAreaSpawnerConfigurationFile LoadConfig(string configPath)
        {
            Log.LogDebug($"Loading SpawnArea spawner configurations from {configPath}.");

            return TomlLoader.LoadFile<SpawnAreaSpawnerConfigurationFile>(configPath);
        }
    }

    private static void CreateSpawnAreaSpawnerFile(string configPath)
    {
        using var file = File.Create(configPath);
        using var writer = new StreamWriter(file);

        writer.WriteLine("# Auto-generated file for adding SpawnArea Spawner configurations.");
        writer.WriteLine("# This file is empty by default. It is intended to contains changes only, to avoid unintentional modifications as well as to reduce unnecessary performance cost.");
        writer.WriteLine("# Full documentation can be found at https://asharppen.github.io/Valheim.SpawnThat.");
        writer.WriteLine("# To get started: ");
        writer.WriteLine($"#     1. Generate default configs in BepInEx/Debug folder, by enabling WriteSpawnTablesToFileBeforeChanges in 'spawn_that.cfg'.");
        writer.WriteLine($"#     2. Start game and enter a world, and wait a short moment (ca. 10 seconds) for files to generate.");
        writer.WriteLine("#     3. Go to generated file, and copy the spawners you want to modify into this file");
        writer.WriteLine("#     4. Make your changes.");
        writer.WriteLine($"# To find modded configs and change those, enable WriteConfigsToFile in 'spawn_that.cfg', and do as described above."); writer.WriteLine();
    }
}
