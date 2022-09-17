using System.Collections.Generic;
using SpawnThat.Integrations;
using SpawnThat.Integrations.EpicLoot.Conditions;
using SpawnThat.Integrations.EpicLoot.Models;

namespace SpawnThat.Spawners;

public static class IHaveSpawnConditionsELConditionsExtensions
{
    public static T SetEpicLootConditionNearbyPlayersCarryItemWithRarity<T>(this T builder, int distanceToSearch, IEnumerable<string> rarities)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            SetConditionItemRarity(builder, distanceToSearch, rarities);
        }

        return builder;
    }

    public static T SetEpicLootConditionNearbyPlayersCarryItemWithRarity<T>(this T builder, int distanceToSearch, params string[] rarities)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            SetConditionItemRarity(builder, distanceToSearch, rarities);
        }

        return builder;
    }

    public static T SetEpicLootConditionNearbyPlayersCarryItemWithRarity<T>(this T builder, int distanceToSearch, IEnumerable<EpicLootRarity> rarities)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            SetConditionItemRarity(builder, distanceToSearch, rarities);
        }

        return builder;
    }

    public static T SetEpicLootConditionNearbyPlayersCarryItemWithRarity<T>(this T builder, int distanceToSearch, params EpicLootRarity[] rarities)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            SetConditionItemRarity(builder, distanceToSearch, rarities);
        }

        return builder;
    }


    private static void SetConditionItemRarity(IHaveSpawnConditions builder, int distance, IEnumerable<string> rarities) => builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distance, rarities));
    private static void SetConditionItemRarity(IHaveSpawnConditions builder, int distance, params string[] rarities) => builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distance, rarities));
    private static void SetConditionItemRarity(IHaveSpawnConditions builder, int distance, IEnumerable<EpicLootRarity> rarities) => builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distance, rarities));
    private static void SetConditionItemRarity(IHaveSpawnConditions builder, int distance, params EpicLootRarity[] rarities) => builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distance, rarities));

    public static T SetEpicLootConditionNearbyPlayerCarryLegendaryItem<T>(this T builder, int distanceToSearch, IEnumerable<string> legendaryItemIds)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            SetConditionLegendaryItem(builder, distanceToSearch, legendaryItemIds);
        }

        return builder;
    }

    public static T SetEpicLootConditionNearbyPlayerCarryLegendaryItem<T>(this T builder, int distanceToSearch, params string[] legendaryItemIds)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            SetConditionLegendaryItem(builder, distanceToSearch, legendaryItemIds);
        }

        return builder;
    }

    private static void SetConditionLegendaryItem(IHaveSpawnConditions builder, int distanceToSearch, IEnumerable<string> legendaryItemIds) => builder.SetCondition(new ConditionNearbyPlayerCarryLegendaryItem(distanceToSearch, legendaryItemIds));
    private static void SetConditionLegendaryItem(IHaveSpawnConditions builder, int distanceToSearch, params string[] legendaryItemIds) => builder.SetCondition(new ConditionNearbyPlayerCarryLegendaryItem(distanceToSearch, legendaryItemIds));
}
