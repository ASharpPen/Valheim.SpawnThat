using System;
using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

namespace SpawnThat.Utilities.Extensions
{
    public static class SpawnConfigurationExtensions
    {
        public static Heightmap.Biome ExtractBiomeMask(this SpawnConfiguration config)
        {
            //Well, since you bastards were packing enums before, lets return the gesture (not really, <3 you devs!)
            Heightmap.Biome biome = Heightmap.Biome.None;

            var biomeArray = config.Biomes?.Value?.SplitByComma() ?? new List<string>(0);

            if (biomeArray.Count == 0)
            {
                //Set all biomes allowed.
                biome = (Heightmap.Biome)1023;
            }

            foreach (var requiredBiome in biomeArray)
            {
                if (Enum.TryParse(requiredBiome, out Heightmap.Biome reqBiome))
                {
                    biome |= reqBiome;
                }
                else
                {
                    Log.LogWarning($"Unable to parse biome '{requiredBiome}' of spawner config {config.Index}");
                }
            }

            return biome;
        }
    }
}
