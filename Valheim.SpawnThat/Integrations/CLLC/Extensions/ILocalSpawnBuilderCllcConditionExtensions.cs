using Valheim.SpawnThat.Integrations;
using Valheim.SpawnThat.Integrations.CLLC.Conditions;
using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;

namespace Valheim.SpawnThat.Spawners;

public static class ILocalSpawnBuilderCllcConditionExtensions
{
    public static ILocalSpawnBuilder AddCllcConditionWorldLevel(this ILocalSpawnBuilder builder, int? minWorldLevel, int? maxWorldLevel)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.AddCondition(new ConditionWorldLevel(minWorldLevel, maxWorldLevel));
        }

        return builder;
    }
}
