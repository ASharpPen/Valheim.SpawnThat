using SpawnThat.Integrations;
using SpawnThat.Integrations.CLLC.Conditions;

namespace SpawnThat.Spawners;

public static class IHaveSpawnConditionsCllcConditionExtensions
{
    public static T SetCllcConditionWorldLevel<T>(this T builder, int? minWorldLevel, int? maxWorldLevel)
        where T : IHaveSpawnConditions
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetCondition(new ConditionWorldLevel(minWorldLevel, maxWorldLevel));
        }

        return builder;
    }
}
