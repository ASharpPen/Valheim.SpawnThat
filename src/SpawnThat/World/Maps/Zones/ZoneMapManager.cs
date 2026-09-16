using System.Collections.Generic;
using System.Diagnostics;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Lifecycle;
using SpawnThat.World.Maps.Images;
using SpawnThat.World.Maps.Images.Colors;
using SpawnThat.World.Maps.Sectors;
using SpawnThat.World.Zone;
using UnityEngine;

namespace SpawnThat.World.Maps.Zones;

internal static class ZoneMapManager
{
    private static bool IsInitialized { get; set; }
    private static int Seed { get; set; }

    public static int Radius { get; set; }

    public static AreaIdMap IdMap { get; set; }

    public static AreaBiomeMap BiomeMap { get; set; }

    public static ZoneBiomeSectorIdMap SectorIdMap { get; set; }

    public static int GridSize { get; set; }

    static ZoneMapManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            IsInitialized = false;
            Seed = default;
            Radius = default;
            IdMap = null;
            BiomeMap = null;
            SectorIdMap = null;
            GridSize = default;
        });
    }

    public static void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }

        if (ZNet.instance.GetWorld() is not null)
        {
            IsInitialized = true;

            Log.LogTrace("Scanning map for biome sectors..");

            var start = Stopwatch.StartNew();

            var sectorWidth = ZNet.instance.GetWorld().m_biomeData.Size;

            Radius = (int)AltBiomeWorldData.MapSpaceToWorldSpace(sectorWidth);
            GridSize = Radius / ZoneUtils.ZoneWidth;

            Seed = WorldGenerator.instance.GetSeed();
            (IdMap, BiomeMap) = new ZoneAreaMapBuilder(Radius).CompileMap();

            start.Stop();

            Log.LogTrace("Scanning map and assigning id's to took: " + start.Elapsed);
        }
    }

    public static void InitializeSectorMap(BiomeSectorIdMap sectorIdMap)
    {
        SectorIdMap = new ZoneBiomeSectorIdMap(sectorIdMap);
    }

    public static int GetAreaId(in Vector3 position)
    {
        var x = IdMap.CoordinateToIndex((int)position.x);
        var y = IdMap.CoordinateToIndex((int)position.y);

        return IdMap.AreaIds[x][y];
    }

    public static float GetAreaChance(in Vector3 position, int modifier = 0)
    {
        var x = IdMap.CoordinateToIndex((int)position.x);
        var y = IdMap.CoordinateToIndex((int)position.y);

        int id = IdMap.AreaIds[x][y];

        System.Random random = new(id + Seed + modifier);

        return (float)random.NextDouble();
    }

    public static float[][] GetTemplateAreaChanceMap(int templateIndex, int scaling = 1)
    {
        float[][] heatmap = new float[IdMap.MapWidth][];

        Dictionary<int, float> chanceById = new();

        for (int x = 0; x < IdMap.MapWidth; ++x)
        {
            heatmap[x] = new float[IdMap.MapWidth];

            for (int y = 0; y < IdMap.MapWidth; ++y)
            {
                int id = IdMap.AreaIds[x][y];

                if (chanceById.ContainsKey(id))
                {
                    heatmap[x][y] = chanceById[id];
                }
                else
                {
                    System.Random rnd = new System.Random(id + Seed + templateIndex);
                    chanceById[id] = (float)rnd.NextDouble() * scaling;
                }
            }
        }

        return heatmap;
    }

    public static void PrintMaps()
    {
        if (ConfigurationManager.GeneralConfig?.PrintAreaMap?.Value == true)
        {
            ZoneMapManager.Initialize();

            ImageBuilder
                .Init(ZoneMapManager.IdMap.GridWidth)
                .Apply(ZoneMapManager.IdMap, RainbowColorMapper.FromIntMap())
                .Print("area_ids_map");

            ImageBuilder
                    .Init(SectorIdMap.GridWidth)
                    .Apply(SectorIdMap, RainbowColorMapper.FromIntMap())
                    .Print("zone_biomesector_ids");
        }

        if (ConfigurationManager.GeneralConfig?.PrintBiomeMap?.Value == true)
        {
            ZoneMapManager.Initialize();

            ImageBuilder
                .Init(ZoneMapManager.BiomeMap.GridWidth)
                .Apply(ZoneMapManager.BiomeMap, BiomeColorMapper.FromIntMap())
                .Print("area_biome_map");
        }
    }
}
