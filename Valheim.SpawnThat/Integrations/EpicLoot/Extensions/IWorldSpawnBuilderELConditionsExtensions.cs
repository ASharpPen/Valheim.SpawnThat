using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Integrations.EpicLoot.Conditions;
using Valheim.SpawnThat.Spawners.WorldSpawner;

namespace Valheim.SpawnThat.Spawners;

public static class IWorldSpawnBuilderELConditionsExtensions
{
    public static bool InstalledEpicLoot { get; } = Type.GetType("EpicLoot.EpicLoot, EpicLoot") is not null;

    public static IWorldSpawnBuilder AddEpicLootConditionNearbyPlayersCarryItemWithRarity(this IWorldSpawnBuilder builder, int distanceToSearch, IEnumerable<string> rarities)
    {
        if (InstalledEpicLoot)
        {
            builder.AddCondition(new ConditionNearbyPlayerCarryItemWithRarity(distanceToSearch, rarities);
        }

        return builder;
    }
}
