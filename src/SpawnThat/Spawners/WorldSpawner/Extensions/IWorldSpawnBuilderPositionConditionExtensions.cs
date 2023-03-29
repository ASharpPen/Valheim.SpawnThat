using System.Collections.Generic;
using System.Linq;
using SpawnThat.Options.PositionConditions;

namespace SpawnThat.Spawners.WorldSpawner;

public static class IWorldSpawnBuilderPositionConditionExtensions
{
    /// <summary>
    /// Sets locations in which spawning is allowed during spawn position checks.
    /// </summary>
    public static IWorldSpawnBuilder SetPositionConditionLocation(this IWorldSpawnBuilder builder, IEnumerable<string> locationNames)
    {
        builder.SetPositionCondition(new PositionConditionLocation(locationNames));
        return builder;
    }

    /// <summary>
    /// Sets locations in which spawning is allowed during spawn position checks.
    /// </summary>
    public static IWorldSpawnBuilder SetPositionConditionLocation(this IWorldSpawnBuilder builder, params string[] locationNames)
    {
        builder.SetPositionCondition(new PositionConditionLocation(locationNames));
        return builder;
    }

    /// <summary>
    /// <para>Set prefabs which are all required to be within distance of position to spawn at.</para>
    /// </summary>
    /// <param name="prefabNames">Names of prefabs that must be present within distance.</param>
    /// <param name="distance">Distance from spawn position to search for prefabs.</param>
    public static IWorldSpawnBuilder ConditionPositionMustBeNearAllPrefabs(this IWorldSpawnBuilder builder, IEnumerable<string> prefabNames, int? distance)
    {
        builder.SetPositionCondition(new PositionConditionMustBeNearAllPrefabs(prefabNames?.ToList() ?? new(), distance));
        return builder;
    }

    /// <summary>
    /// <para>Set prefabs for which at least one is required to be within distance of position to spawn at.</para>
    /// </summary>
    /// <param name="prefabNames">Names of prefabs that will be searched for within distance.</param>
    /// <param name="distance">Distance from spawn position to search for prefabs.</param>
    public static IWorldSpawnBuilder ConditionPositionMustBeNearPrefab(this IWorldSpawnBuilder builder, IEnumerable<string> prefabNames, int? distance)
    {
        builder.SetPositionCondition(new PositionConditionMustBeNearPrefabs(prefabNames?.ToList() ?? new(), distance));
        return builder;
    }

    /// <summary>
    /// <para>Set prefabs for which none must be within distance of position to spawn at.</para>
    /// <para>If any of the listed prefabs are present, spawning is disabled.</para>
    /// </summary>
    /// <param name="prefabNames">Names of prefabs that will be searched for within distance.</param>
    /// <param name="distance">Distance from spawn position to search for prefabs.</param>
    public static IWorldSpawnBuilder ConditionPositionMustNotBeNearPrefab(this IWorldSpawnBuilder builder, IEnumerable<string> prefabNames, int? distance)
    {
        builder.SetPositionCondition(new PositionConditionMustNotBeNearPrefabs(prefabNames?.ToList() ?? new(), distance));
        return builder;
    }
}
