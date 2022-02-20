using SpawnThat.Integrations;
using SpawnThat.Integrations.CLLC.Conditions;

namespace SpawnThat.Spawners.WorldSpawner;

public static class IWorldSpawnBuilderCllcConditionExtensions
{
    public static IWorldSpawnBuilder SetCllcConditionWorldLevel(this IWorldSpawnBuilder builder, int? minWorldLevel, int? maxWorldLevel)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetConditionWorldLevel(builder, minWorldLevel, maxWorldLevel);
        }

        return builder;
    }

    private static void SetConditionWorldLevel(IWorldSpawnBuilder builder, int? minWorldLevel, int? maxWorldLevel) => builder.SetCondition(new ConditionWorldLevel(minWorldLevel, maxWorldLevel));
}
