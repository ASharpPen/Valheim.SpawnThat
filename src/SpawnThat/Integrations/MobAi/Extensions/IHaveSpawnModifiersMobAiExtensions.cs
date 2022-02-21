using SpawnThat.Integrations.MobAi.Modifiers;
using SpawnThat.Integrations;

namespace SpawnThat.Spawners;

public static class IHaveSpawnModifiersMobAiExtensions
{
    public static T SetMobAiModifier<T>(this T builder, string aiName, string mobAiConfig)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledMobAI)
        {
            SetModifier(builder, aiName, mobAiConfig);
        }

        return builder;
    }

    private static void SetModifier(IHaveSpawnModifiers builder, string aiName, string mobAiConfig) => builder.SetModifier(new ModifierSetAI(aiName, mobAiConfig));
}
