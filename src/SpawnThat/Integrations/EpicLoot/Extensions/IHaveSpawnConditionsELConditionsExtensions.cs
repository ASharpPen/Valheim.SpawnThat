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
            builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distanceToSearch, rarities));
        }

        return builder;
    }

    public static T SetEpicLootConditionNearbyPlayersCarryItemWithRarity<T>(this T builder, int distanceToSearch, params string[] rarities)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distanceToSearch, rarities));
        }

        return builder;
    }

    public static T SetEpicLootConditionNearbyPlayersCarryItemWithRarity<T>(this T builder, int distanceToSearch, IEnumerable<EpicLootRarity> rarities)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distanceToSearch, rarities));
        }

        return builder;
    }

    public static T SetEpicLootConditionNearbyPlayersCarryItemWithRarity<T>(this T builder, int distanceToSearch, params EpicLootRarity[] rarities)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.SetCondition(new ConditionNearbyPlayerCarryItemWithRarity(distanceToSearch, rarities));
        }

        return builder;
    }


    public static T SetEpicLootConditionNearbyPlayerCarryLegendaryItem<T>(this T builder, int distanceToSearch, IEnumerable<string> legendaryItemIds)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.SetCondition(new ConditionNearbyPlayerCarryLegendaryItem(distanceToSearch, legendaryItemIds));
        }

        return builder;
    }

    public static T SetEpicLootConditionNearbyPlayerCarryLegendaryItem<T>(this T builder, int distanceToSearch, params string[] legendaryItemIds)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledEpicLoot)
        {
            builder.SetCondition(new ConditionNearbyPlayerCarryLegendaryItem(distanceToSearch, legendaryItemIds));
        }

        return builder;
    }
}
