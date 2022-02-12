using System.Collections.Generic;
using SpawnThat.Integrations;
using SpawnThat.Integrations.EpicLoot.Conditions;
using SpawnThat.Integrations.EpicLoot.Models;

namespace SpawnThat.Spawners.WorldSpawner;

public static class IWorldSpawnBuilderELConditionsExtensions
{
    public static IWorldSpawnBuilder SetEpicLootConditionNearbyPlayersCarryItemWithRarity(this IWorldSpawnBuilder builder, int distanceToSearch, IEnumerable<string> rarities)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distanceToSearch, rarities));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetEpicLootConditionNearbyPlayersCarryItemWithRarity(this IWorldSpawnBuilder builder, int distanceToSearch, params string[] rarities)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distanceToSearch, rarities));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetEpicLootConditionNearbyPlayersCarryItemWithRarity(this IWorldSpawnBuilder builder, int distanceToSearch, IEnumerable<EpicLootRarity> rarities)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distanceToSearch, rarities));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetEpicLootConditionNearbyPlayersCarryItemWithRarity(this IWorldSpawnBuilder builder, int distanceToSearch, params EpicLootRarity[] rarities)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distanceToSearch, rarities));
        }

        return builder;
    }


    public static IWorldSpawnBuilder SetEpicLootConditionNearbyPlayerCarryLegendaryItem(this IWorldSpawnBuilder builder, int distanceToSearch, IEnumerable<string> legendaryItemIds)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.SetCondition(new ConditionNearbyPlayerCarryLegendaryItem(distanceToSearch, legendaryItemIds));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetEpicLootConditionNearbyPlayerCarryLegendaryItem(this IWorldSpawnBuilder builder, int distanceToSearch, params string[] legendaryItemIds)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.SetCondition(new ConditionNearbyPlayerCarryLegendaryItem(distanceToSearch, legendaryItemIds));
        }

        return builder;
    }
}
