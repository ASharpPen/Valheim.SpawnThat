using System.Collections.Generic;
using HarmonyLib;
using SpawnThat.Core;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities;
using SpawnThat.World.Zone;

namespace SpawnThat.Options.Identifiers;

public class IdentifierBiome : ISpawnerIdentifier, ICacheableIdentifier
{
    public Heightmap.Biome BitmaskedBiome { get; set; }

    internal IdentifierBiome()
    {
    }

    public IdentifierBiome(IEnumerable<Heightmap.Biome> biomes)
    {
        BitmaskedBiome = 0;

        foreach (var biome in biomes)
        {
            BitmaskedBiome |= biome;
        }
    }

    public bool IsValid(IdentificationContext context)
    {
        if (BitmaskedBiome == 0 || (int)BitmaskedBiome == int.MaxValue)
        {
            return true;
        }

        var zone = ZoneManager.GetZone(context.Zdo.GetSector());
        var biome = zone.GetBiome(context.Zdo.GetPosition());

#if DEBUG
        Log.LogTrace($"Is '{HeightmapUtils.GetNames(biome)}' in '{HeightmapUtils.GetNames(BitmaskedBiome)}': {(BitmaskedBiome & zone.Biome) > 0}");
#endif

        return (BitmaskedBiome & biome) > 0;
    }

    public long GetParameterHash()
    {
        return (long)BitmaskedBiome;
    }

    public int GetMatchWeight() => MatchWeight.Low;
}
