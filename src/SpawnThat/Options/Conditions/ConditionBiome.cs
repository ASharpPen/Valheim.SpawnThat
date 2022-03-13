using System;
using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Zone;

namespace SpawnThat.Options.Conditions;

public class ConditionBiome : ISpawnCondition
{
    public Heightmap.Biome BiomeMask { get; set; }

    public ConditionBiome(params Heightmap.Biome[] biomes)
    {
        BiomeMask = Heightmap.Biome.None;

        foreach (var biome in biomes)
        {
            BiomeMask |= biome;
        }
    }

    public bool IsValid(SpawnSessionContext sessionContext)
    {
        if (Heightmap.Biome.None == BiomeMask)
        {
            return true;
        }

        var zone = ZoneManager.GetZone(sessionContext.SpawnerZdo.GetSector());

        return zone.HasBiome(BiomeMask);
    }
}
