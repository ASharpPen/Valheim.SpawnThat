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
            SetConditionItemRarity(builder, distanceToSearch, rarities);
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetEpicLootConditionNearbyPlayersCarryItemWithRarity(this IWorldSpawnBuilder builder, int distanceToSearch, params string[] rarities)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            SetConditionItemRarity(builder, distanceToSearch, rarities);
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetEpicLootConditionNearbyPlayersCarryItemWithRarity(this IWorldSpawnBuilder builder, int distanceToSearch, IEnumerable<EpicLootRarity> rarities)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            SetConditionItemRarity(builder, distanceToSearch, rarities);
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetEpicLootConditionNearbyPlayersCarryItemWithRarity(this IWorldSpawnBuilder builder, int distanceToSearch, params EpicLootRarity[] rarities)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            SetConditionItemRarity(builder, distanceToSearch, rarities);
        }

        return builder;
    }

    private static void SetConditionItemRarity(IWorldSpawnBuilder builder, int distance, IEnumerable<string> rarities) => builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distance, rarities));
    private static void SetConditionItemRarity(IWorldSpawnBuilder builder, int distance, params string[] rarities) => builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distance, rarities));
    private static void SetConditionItemRarity(IWorldSpawnBuilder builder, int distance, IEnumerable<EpicLootRarity> rarities) => builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distance, rarities));
    private static void SetConditionItemRarity(IWorldSpawnBuilder builder, int distance, params EpicLootRarity[] rarities) => builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distance, rarities));

    public static IWorldSpawnBuilder SetEpicLootConditionNearbyPlayerCarryLegendaryItem(this IWorldSpawnBuilder builder, int distanceToSearch, IEnumerable<string> legendaryItemIds)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            SetConditionLegendaryItem(builder, distanceToSearch, legendaryItemIds);
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetEpicLootConditionNearbyPlayerCarryLegendaryItem(this IWorldSpawnBuilder builder, int distanceToSearch, params string[] legendaryItemIds)
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            SetConditionLegendaryItem(builder, distanceToSearch, legendaryItemIds);
        }

        return builder;
    }

    private static void SetConditionLegendaryItem(IWorldSpawnBuilder builder, int distanceToSearch, IEnumerable<string> legendaryItemIds) => builder.SetCondition(new ConditionNearbyPlayerCarryLegendaryItem(distanceToSearch, legendaryItemIds));
    private static void SetConditionLegendaryItem(IWorldSpawnBuilder builder, int distanceToSearch, params string[] legendaryItemIds) => builder.SetCondition(new ConditionNearbyPlayerCarryLegendaryItem(distanceToSearch, legendaryItemIds));
}
