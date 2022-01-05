using System.Collections.Generic;
using Valheim.SpawnThat.Spawn.PositionConditions;
using Valheim.SpawnThat.Spawners.WorldSpawner;

namespace Valheim.SpawnThat.Spawners;

public static class IWorldSpawnBuilderPositionConditionExtensions
{
    public static IWorldSpawnBuilder SetPositionConditionLocation(this IWorldSpawnBuilder builder, IEnumerable<string> locationNames)
    {
        builder.AddOrReplacePositionCondition(new PositionConditionLocation(locationNames));
        return builder;
    }

    public static IWorldSpawnBuilder SetPositionConditionLocation(this IWorldSpawnBuilder builder, params string[] locationNames)
    {
        builder.AddOrReplacePositionCondition(new PositionConditionLocation(locationNames));
        return builder;
    }
}
