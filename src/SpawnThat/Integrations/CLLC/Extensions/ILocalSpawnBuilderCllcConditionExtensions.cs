using SpawnThat.Integrations;
using SpawnThat.Integrations.CLLC.Conditions;
using SpawnThat.Spawners.LocalSpawner.Configuration;

namespace SpawnThat.Spawners;

public static class ILocalSpawnBuilderCllcConditionExtensions
{
    public static ILocalSpawnBuilder SetCllcConditionWorldLevel(this ILocalSpawnBuilder builder, int? minWorldLevel, int? maxWorldLevel)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetCondition(new ConditionWorldLevel(minWorldLevel, maxWorldLevel));
        }

        return builder;
    }
}
