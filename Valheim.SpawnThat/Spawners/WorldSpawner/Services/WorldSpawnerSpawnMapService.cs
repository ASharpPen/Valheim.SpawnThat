using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawn.Conditions;
using Valheim.SpawnThat.World.Maps;

namespace Valheim.SpawnThat.Spawners.WorldSpawner.Services;

internal static class WorldSpawnerSpawnMapService
{
    public static int[][] GetMapOfTemplatesActiveAreas(int templateIndex)
    {
        float[][] chanceMap = MapManager.GetTemplateAreaChanceMap(templateIndex, scaling: 100);
        int[][] spawnMap = new int[chanceMap.Length][];

        var template = WorldSpawnTemplateManager.GetTemplate(templateIndex);

        if (template is null)
        {
            Log.LogWarning($"Unable to find config with index '{templateIndex}'");
            return null;
        }

        var allowedBiomes = template.BiomeMask ?? (Heightmap.Biome)1023;

        var locationCondition = template.SpawnConditions.FirstOrDefault(x => x is ConditionLocation) as ConditionLocation;
        var distanceToCenterCondition = template.SpawnConditions.FirstOrDefault(x => x is ConditionDistanceToCenter) as ConditionDistanceToCenter;
        var areaIdsCondition = template.SpawnConditions.FirstOrDefault(x => x is ConditionAreaIds) as ConditionAreaIds;
        var areaSpawnChance = template.SpawnConditions.FirstOrDefault(x => x is ConditionAreaSpawnChance) as ConditionAreaSpawnChance;

        for (int x = 0; x < spawnMap.Length; ++x)
        {
            spawnMap[x] = new int[spawnMap.Length];

            for (int y = 0; y < spawnMap.Length; ++y)
            {
                if ((WeightedCornerBiome(x, y) & (int)allowedBiomes) == 0)
                {
                    continue;
                }

                var coordX = MapManager.AreaMap.IndexToCoordinate(x);
                var coordY = MapManager.AreaMap.IndexToCoordinate(y);

                var position = new Vector3(coordX, 0, coordY);

                if (!locationCondition.IsValid(position))
                {
                    continue;
                }


                if (!distanceToCenterCondition.IsValid(position))
                {
                    continue;
                }

                if (!areaIdsCondition.IsValid(MapManager.AreaMap.AreaIds[x][y]))
                {
                    continue;
                }

                if (chanceMap[x][y] <= (areaSpawnChance?.AreaChance ?? 100))
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

        if (y + 1 < MapManager.AreaMap.Biomes.Length)
        {
            int biome = MapManager.AreaMap.Biomes[x][y + 1];
            biomes.TryGetValue(biome, out var counter);
            biomes[biome] = counter + 1;
        }

        if (x + 1 < MapManager.AreaMap.Biomes.Length)
        {
            int biome = MapManager.AreaMap.Biomes[x + 1][y];
            biomes.TryGetValue(biome, out var counter);
            biomes[biome] = counter + 1;

        }

        if (y - 1 >= 0)
        {
            int biome = MapManager.AreaMap.Biomes[x][y - 1];
            biomes.TryGetValue(biome, out var counter);
            biomes[biome] = counter + 1;
        }

        if (x - 1 >= 0)
        {
            int biome = MapManager.AreaMap.Biomes[x - 1][y];
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
