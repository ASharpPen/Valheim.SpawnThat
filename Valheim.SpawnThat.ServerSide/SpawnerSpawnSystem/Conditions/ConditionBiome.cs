using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;
using Valheim.SpawnThat.Utilities;
using Valheim.SpawnThat.Utilities.World;
using static Heightmap;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions;

public class ConditionBiome : ISpawnCondition
{
    public IReadOnlyList<Biome> Biomes { get; }

    public Biome BitmaskedBiome { get; }

    private static Biome AllBiomesMask = (Biome)int.MaxValue;

    public ConditionBiome(params Biome[] biomes)
    {
        Biomes = biomes.ToList().AsReadOnly();

        foreach(var biome in biomes)
        {
            BitmaskedBiome |= biome;
        }
    }

    public ConditionBiome(string biomeNamesConfig)
    {
        var biomeNames = biomeNamesConfig.SplitByComma();

        List<Biome> biomes = new(biomeNames.Count);

        foreach(var biomeName in biomeNames)
        {
            if(Enum.TryParse(biomeName, true, out Biome biome))
            {
                biomes.Add(biome);
                BitmaskedBiome |= biome;
            }
        }
    }

    public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
    {
        if (BitmaskedBiome == Biome.None || BitmaskedBiome == AllBiomesMask)
        {
            return true;
        }

        var zone = WorldData.GetZone(context.SpawnSystemZDO.m_sector);


#if FALSE && DEBUG
        var hasBiome = zone.HasBiome(BitmaskedBiome);

        if (!hasBiome)
        {
            var zoneBiomes = zone.BiomeCorners.Join((biome) => ((int)biome).ToString(), ", ");
            Log.LogTrace($"Template {template.Index} found biomes '{zoneBiomes}' but required biome " + (int)BitmaskedBiome);
        }

        return hasBiome;
#else
        return zone.HasBiome(BitmaskedBiome);
#endif
    }
}
