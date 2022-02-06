using SpawnThat.Integrations;
using SpawnThat.Integrations.CLLC.Conditions;
using SpawnThat.Spawners.WorldSpawner;

namespace SpawnThat.Spawners;

public static class IWorldSpawnBuilderCllcConditionExtensions
{
    public static IWorldSpawnBuilder SetCllcConditionWorldLevel(this IWorldSpawnBuilder builder, int? minWorldLevel, int? maxWorldLevel)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetCondition(new ConditionWorldLevel(minWorldLevel, maxWorldLevel));
        }

        return builder;
    }
}
