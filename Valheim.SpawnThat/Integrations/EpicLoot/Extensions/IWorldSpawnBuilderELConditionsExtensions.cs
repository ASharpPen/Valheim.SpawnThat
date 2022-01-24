using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Integrations;
using Valheim.SpawnThat.Integrations.EpicLoot.Conditions;
using Valheim.SpawnThat.Integrations.EpicLoot.Models;
using Valheim.SpawnThat.Spawners.WorldSpawner;

namespace Valheim.SpawnThat.Spawners;

public static class IWorldSpawnBuilderELConditionsExtensions
{
    public static IWorldSpawnBuilder AddEpicLootConditionNearbyPlayersCarryItemWithRarity(this IWorldSpawnBuilder builder, int distanceToSearch, IEnumerable<string> rarities)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.AddCondition(new ConditionNearbyPlayerCarryItemWithRarity(distanceToSearch, rarities));
        }

        return builder;
    }

    public static IWorldSpawnBuilder AddEpicLootConditionNearbyPlayersCarryItemWithRarity(this IWorldSpawnBuilder builder, int distanceToSearch, IEnumerable<EpicLootRarity> rarities)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.AddCondition(new ConditionNearbyPlayerCarryItemWithRarity(distanceToSearch, rarities));
        }

        return builder;
    }

    public static IWorldSpawnBuilder AddEpicLootConditionNearbyPlayerCarryLegendaryItem(this IWorldSpawnBuilder builder, int distanceToSearch, IEnumerable<string> legendaryItemIds)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.AddCondition(new ConditionNearbyPlayerCarryLegendaryItem(distanceToSearch, legendaryItemIds));
        }

        return builder;
    }

    public static IWorldSpawnBuilder AddEpicLootConditionNearbyPlayerCarryLegendaryItem(this IWorldSpawnBuilder builder, int distanceToSearch, params string[] legendaryItemIds)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.AddCondition(new ConditionNearbyPlayerCarryLegendaryItem(distanceToSearch, legendaryItemIds));
        }

        return builder;
    }
}
