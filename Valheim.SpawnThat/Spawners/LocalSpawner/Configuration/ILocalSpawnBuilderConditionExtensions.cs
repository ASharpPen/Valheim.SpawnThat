using Valheim.SpawnThat.Spawners.Conditions;
using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;

namespace Valheim.SpawnThat.Spawners;

public static class ILocalSpawnBuilderConditionExtensions
{
    public static ILocalSpawnBuilder SetConditionCloseToPlayer(this ILocalSpawnBuilder builder, float withinDistance)
    {
        builder.AddOrReplaceCondition(new ConditionCloseToPlayer(withinDistance));
        return builder;
    }
}
