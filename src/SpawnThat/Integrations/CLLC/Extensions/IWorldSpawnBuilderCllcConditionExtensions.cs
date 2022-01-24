using SpawnThat.Integrations;
using SpawnThat.Integrations.CLLC.Conditions;
using SpawnThat.Spawners.WorldSpawner;

namespace SpawnThat.Spawners;

public static class IWorldSpawnBuilderCllcConditionExtensions
{
    public static IWorldSpawnBuilder AddCllcConditionWorldLevel(this IWorldSpawnBuilder builder, int? minWorldLevel, int? maxWorldLevel)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.AddCondition(new ConditionWorldLevel(minWorldLevel, maxWorldLevel));
        }

        return builder;
    }
}
