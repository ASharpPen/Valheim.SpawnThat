using System;
using System.Collections.Generic;
using UnityEngine;
using SpawnThat.World.Maps.Area;

namespace SpawnThat.World.Maps;

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
}
