using SpawnThat.Integrations;
using SpawnThat.Integrations.CLLC.Conditions;

namespace SpawnThat.Spawners.LocalSpawner;

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
