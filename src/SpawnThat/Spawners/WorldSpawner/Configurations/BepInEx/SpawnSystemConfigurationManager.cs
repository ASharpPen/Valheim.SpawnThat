using BepInEx;
using BepInEx.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Core.Configuration;
using SpawnThat.Core.Toml;

namespace SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

internal static class SpawnSystemConfigurationManager
{
    public static SpawnSystemConfigurationFile SpawnSystemConfig;
    public static SimpleConfigurationFile SimpleConfig;

    internal const string SimpleConfigFile = "spawn_that.simple.cfg";
    internal const string SpawnSystemConfigFile = "spawn_that.world_spawners_advanced.cfg";

    internal const string SpawnSystemSupplemental = "spawn_that.world_spawners.*.cfg";

    public static void LoadAllConfigurations()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        SimpleConfig = LoadSimpleConfig();
        SpawnSystemConfig = LoadSpawnSystemConfiguration();

        stopwatch.Stop();

        Log.LogInfo("Loading world spawner configs took: " + stopwatch.Elapsed);
    }

    public static SimpleConfigurationFile LoadSimpleConfig()
    {
        Log.LogInfo("Loading world spawner simple configurations");

        string configPath = Path.Combine(Paths.ConfigPath, SimpleConfigFile);

        if (!File.Exists(configPath) && ConfigurationManager.GeneralConfig?.InitializeWithCreatures?.Value == true)
        {
            SimpleConfigPreconfiguration.Initialize();
        }

        return TomlLoader.LoadFile<SimpleConfigurationFile>(configPath);
    }

    public static SpawnSystemConfigurationFile LoadSpawnSystemConfiguration()
    {
        Log.LogInfo($"Loading world spawner configurations.");

        string configPath = Path.Combine(Paths.ConfigPath, SpawnSystemConfigFile);

        if (!File.Exists(configPath))
        {
            CreateDefaultWorldSpawnerFile(configPath);
        }

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

        return TomlLoader.LoadFile<SpawnSystemConfigurationFile>(configPath);
    }

    private static void CreateDefaultWorldSpawnerFile(string configPath)
    {
        using var file = File.Create(configPath);
        using var writer = new StreamWriter(file);

        writer.WriteLine("# Auto-generated file for adding World Spawner configurations.");
        writer.WriteLine("# This file is empty by default. It is intended to contains changes only, to avoid unintentional modifications as well as to reduce unnecessary performance cost.");
        writer.WriteLine("# Full documentation can be found at https://asharppen.github.io/Valheim.SpawnThat.");
        writer.WriteLine("# To get started: ");
        writer.WriteLine($"#     1. Generate default configs in BepInEx/Debug folder, by enabling {nameof(GeneralConfiguration.WriteSpawnTablesToFileBeforeChanges)} in 'spawn_that.cfg'.");
        writer.WriteLine($"#     2. Start game and enter a world, and wait a short moment (ca. 10 seconds) for files to generate.");
        writer.WriteLine("#     3. Go to generated file, and copy the creatures you want to modify into this file");
        writer.WriteLine("#     4. Make your changes.");
        writer.WriteLine($"# To find modded configs and change those, enable {nameof(GeneralConfiguration.WriteSpawnTablesToFileAfterChanges)} in 'spawn_that.cfg', and do as described above.");
        writer.WriteLine();
    }
}
