using SpawnThat.Integrations;
using SpawnThat.Integrations.MobAi.Modifiers;

namespace SpawnThat.Spawners.WorldSpawner;

public static class IWorldSpawnBuilderMobAiExtensions
{
    public static IWorldSpawnBuilder SetMobAiModifier(this IWorldSpawnBuilder builder, string aiName, string mobAiConfig)
    {
        if (IntegrationManager.InstalledMobAI)
        {
            builder.SetModifier(new ModifierSetAI(aiName, mobAiConfig));
        }

        return builder;
    }
}
