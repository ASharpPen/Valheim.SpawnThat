using System.Collections.Generic;
using SpawnThat.Integrations;
using SpawnThat.Integrations.EpicLoot.Conditions;
using SpawnThat.Integrations.EpicLoot.Models;
using SpawnThat.Spawners.WorldSpawner;

namespace SpawnThat.Spawners;

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
