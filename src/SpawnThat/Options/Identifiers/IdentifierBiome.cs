using System.Collections.Generic;
using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Zone;

namespace SpawnThat.Options.Identifiers;

internal class IdentifierBiome : ISpawnerIdentifier, ICacheableIdentifier
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

        var zone = ZoneManager.GetZone(context.Zdo.m_sector);

        return zone.HasBiome(BitmaskedBiome);
    }

    public long GetParameterHash()
    {
        return (long)BitmaskedBiome;
    }

    public int GetMatchWeight() => MatchWeight.Low;
}
