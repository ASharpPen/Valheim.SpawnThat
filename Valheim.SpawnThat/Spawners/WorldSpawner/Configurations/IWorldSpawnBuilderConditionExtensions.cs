using Valheim.SpawnThat.Spawners.Conditions;
using Valheim.SpawnThat.Spawners.WorldSpawner;

namespace Valheim.SpawnThat.Spawners;

public static class IWorldSpawnBuilderConditionExtensions
{
    public static IWorldSpawnBuilder SetConditionCloseToPlayer(this IWorldSpawnBuilder builder, float withinDistance)
    {
        builder.AddOrReplaceCondition(new ConditionCloseToPlayer(withinDistance));
        return builder;
    }
}

