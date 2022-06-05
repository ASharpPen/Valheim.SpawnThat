using System;
using System.Diagnostics;
using System.IO;
using BepInEx;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Core.Toml;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration.BepInEx;

internal static class DestructibleSpawnerTomlCfgManager
{
    private const string ConfigFile = "spawn_that.destructible_spawners.cfg";
    private const string ConfigFileSupplemental = "spawn_that.destructible_spawners.*.cfg";

    internal static DestructibleSpawnerConfigurationFile Config { get; private set; }

    public static void Load()
    {
        try
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Log.LogInfo($"Loading destructible spawner configurations.");

            string configPath = Path.Combine(Paths.ConfigPath, ConfigFile);

            if (!File.Exists(configPath))
            {
                CreateDestructibleSpawnerFile(configPath);
            }

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

            Log.LogInfo("Loading destructible spawner configs took: " + stopwatch.Elapsed);

            Config = configs;
        }
        catch (Exception e)
        {
            Log.LogError("Error during loading of destructible spawner cfg files.", e);
        }

        static DestructibleSpawnerConfigurationFile LoadConfig(string configPath)
        {
            Log.LogDebug($"Loading destructible spawner configurations from {configPath}.");

            return TomlLoader.LoadFile<DestructibleSpawnerConfigurationFile>(configPath);
        }
    }

    private static void CreateDestructibleSpawnerFile(string configPath)
    {
        using var file = File.Create(configPath);
        using var writer = new StreamWriter(file);

        writer.WriteLine("# Auto-generated file for adding Destructible Spawner configurations.");
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
