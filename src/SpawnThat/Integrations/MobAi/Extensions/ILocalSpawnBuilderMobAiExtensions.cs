using SpawnThat.Integrations.MobAi.Modifiers;
using SpawnThat.Integrations;

namespace SpawnThat.Spawners.LocalSpawner;

public static class ILocalSpawnBuilderMobAiExtensions
{
    public static ILocalSpawnBuilder SetMobAiModifier(this ILocalSpawnBuilder builder, string aiName, string mobAiConfig)
    {
        if (IntegrationManager.InstalledMobAI)
        {
            SetModifier(builder, aiName, mobAiConfig);
        }

        return builder;
    }

    private static void SetModifier(ILocalSpawnBuilder builder, string aiName, string mobAiConfig) => builder.SetModifier(new ModifierSetAI(aiName, mobAiConfig));
}
