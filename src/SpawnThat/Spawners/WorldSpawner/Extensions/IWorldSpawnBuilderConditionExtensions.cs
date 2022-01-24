using System.Collections.Generic;
using System.Linq;
using SpawnThat.Options.Conditions;
using SpawnThat.Spawners.WorldSpawner;

namespace SpawnThat.Spawners;

public static class IWorldSpawnBuilderConditionExtensions
{
    public static IWorldSpawnBuilder SetConditionAreaIds(this IWorldSpawnBuilder builder, IEnumerable<int> areaIds)
    {
        builder.AddOrReplaceCondition(new ConditionAreaIds(areaIds.ToList()));
        return builder;
    }

    public static IWorldSpawnBuilder SetConditionAreaIds(this IWorldSpawnBuilder builder, params int[] areaIds)
    {
        builder.AddOrReplaceCondition(new ConditionAreaIds(areaIds.ToList()));
        return builder;
    }

    // TODO: Add some internal state for the id, so user doesn't need to specify it.
    public static IWorldSpawnBuilder SetConditionAreaSpawnChance(this IWorldSpawnBuilder builder, double areaChance, uint templateId)
    {
        builder.AddOrReplaceCondition(new ConditionAreaSpawnChance(areaChance, (int)templateId));
        return builder;
    }

    public static IWorldSpawnBuilder SetConditionCloseToPlayer(this IWorldSpawnBuilder builder, float withinDistance)
    {
        builder.AddOrReplaceCondition(new ConditionCloseToPlayer(withinDistance));
        return builder;
    }

    public static IWorldSpawnBuilder SetConditionDistanceToCenter(this IWorldSpawnBuilder builder, double? minDistance = null, double? maxDistance = null)
    {
        builder.AddOrReplaceCondition(new ConditionDistanceToCenter(minDistance, maxDistance));
        return builder;
    }

    public static IWorldSpawnBuilder SetGlobalKeysRequiredMissing(this IWorldSpawnBuilder builder, IEnumerable<string> globalKeys)
    {
        builder.AddOrReplaceCondition(new ConditionGlobalKeysRequiredMissing(globalKeys.ToArray()));
        return builder;
    }

    public static IWorldSpawnBuilder SetGlobalKeysRequiredMissing(this IWorldSpawnBuilder builder, params string[] globalKeys)
    {
        builder.AddOrReplaceCondition(new ConditionGlobalKeysRequiredMissing(globalKeys));
        return builder;
    }

    public static IWorldSpawnBuilder SetConditionLocation(this IWorldSpawnBuilder builder, IEnumerable<string> locationNames)
    {
        builder.AddOrReplaceCondition(new ConditionLocation(locationNames.ToArray()));
        return builder;
    }

    public static IWorldSpawnBuilder SetConditionLocation(this IWorldSpawnBuilder builder, params string[] locationNames)
    {
        builder.AddOrReplaceCondition(new ConditionLocation(locationNames));
        return builder;
    }

    public static IWorldSpawnBuilder AddConditionNearbyPlayersCarryItem(this IWorldSpawnBuilder builder, int distance, IEnumerable<string> itemPrefabNames)
    {
        builder.AddCondition(new ConditionNearbyPlayersCarryItem(distance, itemPrefabNames.ToArray()));
        return builder;
    }

    public static IWorldSpawnBuilder AddConditionNearbyPlayersCarryItem(this IWorldSpawnBuilder builder, int distance, params string[] itemPrefabNames)
    {
        builder.AddCondition(new ConditionNearbyPlayersCarryItem(distance, itemPrefabNames));
        return builder;
    }

    public static IWorldSpawnBuilder SetConditionNearbyPlayersCarryValue(this IWorldSpawnBuilder builder, int distance, int combinedValueRequired)
    {
        builder.AddOrReplaceCondition(new ConditionNearbyPlayersCarryValue(distance, combinedValueRequired));
        return builder;
    }

    public static IWorldSpawnBuilder SetConditionNearbyPlayersNoise(this IWorldSpawnBuilder builder, int distance, float noiseRequired)
    {
        builder.AddOrReplaceCondition(new ConditionNearbyPlayersNoise(distance, noiseRequired));
        return builder;
    }

    public static IWorldSpawnBuilder AddConditionNearbyPlayersStatus(this IWorldSpawnBuilder builder, int distance, IEnumerable<string> statusEffectNames)
    {
        builder.AddCondition(new ConditionNearbyPlayersStatus(distance, statusEffectNames.ToArray()));
        return builder;
    }

    public static IWorldSpawnBuilder AddConditionNearbyPlayersStatus(this IWorldSpawnBuilder builder, int distance, params string[] statusEffectNames)
    {
        builder.AddCondition(new ConditionNearbyPlayersStatus(distance, statusEffectNames));
        return builder;
    }

    public static IWorldSpawnBuilder SetConditionWorldAge(this IWorldSpawnBuilder builder, int? minDays = null, int? maxDays = null)
    {
        builder.AddOrReplaceCondition(new ConditionWorldAge(minDays, maxDays));
        return builder;
    }
}

