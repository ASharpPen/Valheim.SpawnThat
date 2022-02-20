using SpawnThat.Integrations;
using SpawnThat.Integrations.MobAi.Modifiers;

namespace SpawnThat.Spawners.WorldSpawner;

public static class IWorldSpawnBuilderMobAiExtensions
{
    public static IWorldSpawnBuilder SetMobAiModifier(this IWorldSpawnBuilder builder, string aiName, string mobAiConfig)
    {
        if (IntegrationManager.InstalledMobAI)
        {
            SetModifier(builder, aiName, mobAiConfig);
        }

        return builder;
    }

    private static void SetModifier(IWorldSpawnBuilder builder, string aiName, string mobAiConfig) => builder.SetModifier(new ModifierSetAI(aiName, mobAiConfig));
}
