using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawn.Conditions;
using Valheim.SpawnThat.Utilities.Extensions;
using Valheim.SpawnThat.World.Maps.Area;

namespace Valheim.SpawnThat.World.Maps;

public static class MapManager
{
    internal static AreaMap AreaMap { get; set; }

    private static int Seed { get; set; }

    public static void Initialize()
    {
        Seed = WorldGenerator.instance.GetSeed();
        AreaMap = AreaMapBuilder
            .BiomeMap(10500)
            .CompileMap();
    }

    public static int GetAreaId(Vector3 position)
    {
        int x = Math.Min(AreaMap.MapWidth, Math.Max(0, AreaMap.CoordinateToIndex((int)position.x)));
        int y = Math.Min(AreaMap.MapWidth, Math.Max(0, AreaMap.CoordinateToIndex((int)position.z)));

        return AreaMap.AreaIds[x][y];
    }

    public static float GetAreaChance(Vector3 position, int modifier = 0)
    {
        int x = Math.Min(AreaMap.MapWidth, Math.Max(0, AreaMap.CoordinateToIndex((int)position.x)));
        int y = Math.Min(AreaMap.MapWidth, Math.Max(0, AreaMap.CoordinateToIndex((int)position.z)));

        int id = AreaMap.AreaIds[x][y];

        System.Random random = new(id + Seed + modifier);

        return (float)random.NextDouble();
    }

    public static float[][] GetTemplateAreaChanceMap(int templateIndex, int scaling = 1)
    {
        float[][] heatmap = new float[AreaMap.MapWidth][];

        Dictionary<int, float> chanceById = new();

        for (int x = 0; x < AreaMap.MapWidth; ++x)
        {
            heatmap[x] = new float[AreaMap.MapWidth];

            for (int y = 0; y < AreaMap.MapWidth; ++y)
            {
                int id = AreaMap.AreaIds[x][y];

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

    public static int[][] GetSpawnMap(int templateIndex)
    {
        float[][] chanceMap = GetTemplateAreaChanceMap(templateIndex, scaling: 100);
        int[][] spawnMap = new int[chanceMap.Length][];

        //SpawnSystem config is only expected to have a single first layer, namely "WorldSpawner", so we just grab the first entry.
        var spawnSystemConfigs = ConfigurationManager
            .SpawnSystemConfig?
            .Subsections? //[*]
            .Values?
            .FirstOrDefault();

        if (!spawnSystemConfigs.Subsections.TryGetValue(templateIndex.ToString(), out SpawnConfiguration config))
        {
            Log.LogWarning($"Unable to find config with index '{templateIndex}'");
            return null;
        }

#if FALSE && DEBUG
            var indexX = AreaMap.CoordinateToIndex(-76);
            var indexY = AreaMap.CoordinateToIndex(384);

            Log.LogDebug($"Runestone_Boars map index: ({indexX}, {indexY})");

            var locX = AreaMap.IndexToCoordinate(indexX);
            var locY = AreaMap.IndexToCoordinate(indexY);

            Log.LogDebug($"Re-estimated coordinates: ({locX}, {locY})");

            var zoneId = ZoneSystem.instance.GetZone(new Vector3(locX + 1, 0, locY + 1));
            var realZoneId = ZoneSystem.instance.GetZone(new Vector3(-76.9f, 0, 384.7f));

            Log.LogDebug($"ZoneId:      {zoneId}");
            Log.LogDebug($"Real ZoneId: {realZoneId}");
#endif

        var allowedBiomes = config.ExtractBiomeMask();

        for (int x = 0; x < spawnMap.Length; ++x)
        {
            spawnMap[x] = new int[spawnMap.Length];

            for (int y = 0; y < spawnMap.Length; ++y)
            {
                if ((WeightedCornerBiome(x, y) & (int)allowedBiomes) == 0)
                {
                    continue;
                }

                var coordX = AreaMap.IndexToCoordinate(x);
                var coordY = AreaMap.IndexToCoordinate(y);

                var position = new Vector3(coordX, 0, coordY);

                if (!ConditionLocation.Instance.IsValid(position, config))
                {
                    continue;
                }

                if (!ConditionDistanceToCenter.Instance.IsValid(position, config))
                {
                    continue;
                }

                if (!ConditionAreaIds.Instance.IsValid(AreaMap.AreaIds[x][y], config))
                {
                    continue;
                }

                if (chanceMap[x][y] <= config.ConditionAreaSpawnChance.Value)
                {
                    spawnMap[x][y] = 255;
                }
            }
        }

        return spawnMap;
    }

    private static int WeightedCornerBiome(int x, int y)
    {
        Dictionary<int, int> biomes = new();

        if (y + 1 < AreaMap.Biomes.Length)
        {
            int biome = AreaMap.Biomes[x][y + 1];
            biomes.TryGetValue(biome, out var counter);
            biomes[biome] = counter + 1;
        }

        if (x + 1 < AreaMap.Biomes.Length)
        {
            int biome = AreaMap.Biomes[x + 1][y];
            biomes.TryGetValue(biome, out var counter);
            biomes[biome] = counter + 1;

        }

        if (y - 1 >= 0)
        {
            int biome = AreaMap.Biomes[x][y - 1];
            biomes.TryGetValue(biome, out var counter);
            biomes[biome] = counter + 1;
        }

        if (x - 1 >= 0)
        {
            int biome = AreaMap.Biomes[x - 1][y];
            biomes.TryGetValue(biome, out var counter);
            biomes[biome] = counter + 1;

        }

        return biomes
            .OrderByDescending(x => x.Value)
            .ThenBy(x => x.Key)
            .FirstOrDefault()
            .Key;
    }
}
