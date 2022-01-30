using System.Collections.Generic;
using SpawnThat.Options.PositionConditions;
using SpawnThat.Spawners.WorldSpawner;

namespace SpawnThat.Spawners;

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
}
