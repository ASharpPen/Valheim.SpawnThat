using System.Collections.Generic;
using System.Linq;
using SpawnThat.Options.Conditions;
using SpawnThat.Spawners.LocalSpawner.Configuration;

namespace SpawnThat.Spawners;

public static class ILocalSpawnBuilderConditionExtensions
{
    public static ILocalSpawnBuilder SetConditionAreaIds(this ILocalSpawnBuilder builder, IEnumerable<int> areaIds)
    {
        builder.AddOrReplaceCondition(new ConditionAreaIds(areaIds.ToList()));
        return builder;
    }

    public static ILocalSpawnBuilder SetConditionAreaIds(this ILocalSpawnBuilder builder, params int[] areaIds)
    {
        builder.AddOrReplaceCondition(new ConditionAreaIds(areaIds.ToList()));
        return builder;
    }

    // TODO: Add some internal state for the id, so user doesn't need to specify it.
    public static ILocalSpawnBuilder SetConditionAreaSpawnChance(this ILocalSpawnBuilder builder, double areaChance, uint templateId)
    {
        builder.AddOrReplaceCondition(new ConditionAreaSpawnChance(areaChance, (int)templateId));
        return builder;
    }

    public static ILocalSpawnBuilder SetConditionCloseToPlayer(this ILocalSpawnBuilder builder, float withinDistance)
    {
        builder.AddOrReplaceCondition(new ConditionCloseToPlayer(withinDistance));
        return builder;
    }

    public static ILocalSpawnBuilder SetConditionDistanceToCenter(this ILocalSpawnBuilder builder, double? minDistance = null, double? maxDistance = null)
    {
        builder.AddOrReplaceCondition(new ConditionDistanceToCenter(minDistance, maxDistance));
        return builder;
    }

    public static ILocalSpawnBuilder SetGlobalKeysRequiredMissing(this ILocalSpawnBuilder builder, IEnumerable<string> globalKeys)
    {
        builder.AddOrReplaceCondition(new ConditionGlobalKeysRequiredMissing(globalKeys.ToArray()));
        return builder;
    }

    public static ILocalSpawnBuilder SetConditionLocation(this ILocalSpawnBuilder builder, IEnumerable<string> locationNames)
    {
        builder.AddOrReplaceCondition(new ConditionLocation(locationNames.ToArray()));
        return builder;
    }

    public static ILocalSpawnBuilder SetConditionLocation(this ILocalSpawnBuilder builder, params string[] locationNames)
    {
        builder.AddOrReplaceCondition(new ConditionLocation(locationNames));
        return builder;
    }

    public static ILocalSpawnBuilder AddConditionNearbyPlayersCarryItem(this ILocalSpawnBuilder builder, int distance, IEnumerable<string> itemPrefabNames)
    {
        builder.AddCondition(new ConditionNearbyPlayersCarryItem(distance, itemPrefabNames.ToArray()));
        return builder;
    }

    public static ILocalSpawnBuilder AddConditionNearbyPlayersCarryItem(this ILocalSpawnBuilder builder, int distance, params string[] itemPrefabNames)
    {
        builder.AddCondition(new ConditionNearbyPlayersCarryItem(distance, itemPrefabNames));
        return builder;
    }

    public static ILocalSpawnBuilder SetConditionNearbyPlayersCarryValue(this ILocalSpawnBuilder builder, int distance, int combinedValueRequired)
    {
        builder.AddOrReplaceCondition(new ConditionNearbyPlayersCarryValue(distance, combinedValueRequired));
        return builder;
    }

    public static ILocalSpawnBuilder SetConditionNearbyPlayersNoise(this ILocalSpawnBuilder builder, int distance, int noiseRequired)
    {
        builder.AddOrReplaceCondition(new ConditionNearbyPlayersNoise(distance, noiseRequired));
        return builder;
    }

    public static ILocalSpawnBuilder AddConditionNearbyPlayersStatus(this ILocalSpawnBuilder builder, int distance, IEnumerable<string> statusEffectNames)
    {
        builder.AddCondition(new ConditionNearbyPlayersStatus(distance, statusEffectNames.ToArray()));
        return builder;
    }

    public static ILocalSpawnBuilder AddConditionNearbyPlayersStatus(this ILocalSpawnBuilder builder, int distance, params string[] statusEffectNames)
    {
        builder.AddCondition(new ConditionNearbyPlayersStatus(distance, statusEffectNames));
        return builder;
    }

    public static ILocalSpawnBuilder SetConditionWorldAge(this ILocalSpawnBuilder builder, int? minDays = null, int? maxDays = null)
    {
        builder.AddOrReplaceCondition(new ConditionWorldAge(minDays, maxDays));
        return builder;
    }
}
