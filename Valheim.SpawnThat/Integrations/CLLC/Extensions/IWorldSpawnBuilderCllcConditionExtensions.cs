using Valheim.SpawnThat.Integrations;
using Valheim.SpawnThat.Integrations.CLLC.Conditions;
using Valheim.SpawnThat.Spawners.WorldSpawner;

namespace Valheim.SpawnThat.Spawners;

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
