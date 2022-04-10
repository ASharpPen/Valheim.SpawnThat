using System;
using System.Collections.Generic;
using SpawnThat.Core.Configuration;
using SpawnThat.Core;
using static Heightmap;
using System.Linq;

namespace SpawnThat.Utilities;

internal static class HeightmapUtils
{
    public static Heightmap.Biome ParseBiomeMask(IEnumerable<string> biomeNames)
    {
        Heightmap.Biome result = Biome.None;

        if (biomeNames?.Any() != true)
        {
            return result;
        }

        foreach (var biomeName in biomeNames)
        {
            if (Enum.TryParse(biomeName, true, out Heightmap.Biome biome))
            {
                result |= biome;
            }
            else
            {
                Log.LogWarning($"Unable to parse biome '{biomeName}'. Check spelling.");
            }
        }

        return result;
    }

    public static List<Heightmap.Biome> ParseBiomes(IEnumerable<string> biomeNames)
    {
        List<Heightmap.Biome> results = new();

        if (biomeNames?.Any() != true)
        {
            return results;
        }

        foreach (var biomeName in biomeNames)
        {
            if (Enum.TryParse(biomeName, true, out Heightmap.Biome biome))
            {
                results.Add(biome);
            }
            else
            {
                Log.LogWarning($"Unable to parse biome '{biomeName}'. Check spelling.");
            }
        }

        return results;
    }
}
