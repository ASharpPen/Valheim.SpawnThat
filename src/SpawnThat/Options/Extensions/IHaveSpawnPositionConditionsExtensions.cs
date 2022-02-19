using SpawnThat.Options.PositionConditions;
using System.Collections.Generic;

namespace SpawnThat.Spawners;

public static class IHaveSpawnPositionConditionsExtensions
{
    /// <summary>
    /// Sets locations in which spawning is allowed during spawn position checks.
    /// </summary>
    public static T SetPositionConditionLocation<T>(this T builder, IEnumerable<string> locationNames)
        where T : IHaveSpawnPositionConditions
    {
        builder.SetPositionCondition(new PositionConditionLocation(locationNames));
        return builder;
    }

    /// <summary>
    /// Sets locations in which spawning is allowed during spawn position checks.
    /// </summary>
    public static T SetPositionConditionLocation<T>(this T builder, params string[] locationNames)
        where T : IHaveSpawnPositionConditions
    {
        builder.SetPositionCondition(new PositionConditionLocation(locationNames));
        return builder;
    }
}
