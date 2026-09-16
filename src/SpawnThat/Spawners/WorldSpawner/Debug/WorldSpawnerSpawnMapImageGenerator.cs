using System;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.WorldSpawner.Managers;
using SpawnThat.Spawners.WorldSpawner.Services;
using SpawnThat.World.Maps;
using SpawnThat.World.Maps.Images;
using SpawnThat.World.Maps.Images.Colors;
using SpawnThat.World.Maps.Zones;

namespace SpawnThat.Spawners.WorldSpawner.Debug;

internal static class WorldSpawnerSpawnMapImageGenerator
{
    private static bool BiomesLoaded;
    private static bool ConfigsApplied;
    private static bool HasRun;

    public static void Configure()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            BiomesLoaded = false;
            ConfigsApplied = false;
            HasRun = false;
        });
    }

    public static void OnConfigsReady()
    {
        ConfigsApplied = true;
        PrintMaps();
    }

    public static void OnBiomesLoaded()
    {
        BiomesLoaded = true;
        PrintMaps();
    }

    private static void PrintMaps()
    {
        try
        {
            if (HasRun || 
                !BiomesLoaded ||
                !ConfigsApplied)
            {
                return;
            }

            HasRun = true;

            if (ConfigurationManager.GeneralConfig?.PrintFantasticBeastsAndWhereToKillThem?.Value == true)
            {
                ZoneMapManager.Initialize();

                foreach (var config in WorldSpawnTemplateManager.GetTemplates())
                {
                    if (!(config.template.Enabled))
                    {
                        continue;
                    }

                    var spawnMap = WorldSpawnerSpawnMapService.GetMapOfTemplatesActiveAreas(config.id);

                    if (spawnMap is null)
                    {
                        continue;
                    }

                    ImageBuilder
                        .Init(ZoneMapManager.BiomeMap.GridWidth)
                        .Apply(ZoneMapManager.BiomeMap, BiomeGrayscaleColorMapper.FromIntMap())
                        .Apply(
                            IntMap.FromGrid(spawnMap), 
                            x => x != 0,
                            HeatColorMapper.FromIntMap())
                        .Print($"spawn_map_{config.id}_{config.template.PrefabName ?? config.template.TemplateName ?? string.Empty}");
                }
            }
        }
        catch (Exception e)
        {
            Log.LogWarning("Error while attempting to print spawn area maps. Skipping map printing.", e);
        }
    }
}
