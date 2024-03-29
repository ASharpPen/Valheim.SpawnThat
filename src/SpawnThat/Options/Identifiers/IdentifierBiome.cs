﻿using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities;
using SpawnThat.World.Zone;

namespace SpawnThat.Options.Identifiers;

public class IdentifierBiome : ISpawnerIdentifier
{
    public Heightmap.Biome BitmaskedBiome { get; set; }

    public IdentifierBiome()
    {
    }

    public IdentifierBiome(IEnumerable<Heightmap.Biome> biomes)
    {
        BitmaskedBiome = 0;

        if (biomes is not null)
        {
            foreach (var biome in biomes)
            {
                BitmaskedBiome |= biome;
            }
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

    public bool Equals(ISpawnerIdentifier other)
    {
        if (ReferenceEquals(other, this))
        {
            return true;
        }

        return
            other is IdentifierBiome otherIdentifier &&
            otherIdentifier.BitmaskedBiome == BitmaskedBiome;
    }
}
