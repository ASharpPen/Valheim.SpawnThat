using System;
using System.Collections.Generic;
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

    public static string GetNames(Heightmap.Biome biome)
    {
        List<string> biomes = new List<string>();

        foreach (Heightmap.Biome potentialBiome in Enum.GetValues(typeof(Heightmap.Biome)))
        {
            if (potentialBiome == Heightmap.Biome.BiomesMax || potentialBiome == Heightmap.Biome.None)
            {
                continue;
            }

            if ((biome & potentialBiome) > 0)
            {
                biomes.Add(potentialBiome.ToString());
            }
        }

        return string.Join(", ", biomes);
    }
}
