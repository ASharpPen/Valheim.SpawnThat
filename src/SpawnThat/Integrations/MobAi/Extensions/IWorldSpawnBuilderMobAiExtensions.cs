using SpawnThat.Integrations;
using SpawnThat.Integrations.MobAi.Modifiers;
using SpawnThat.Spawners.WorldSpawner;

namespace SpawnThat.Spawners;

public static class IWorldSpawnBuilderMobAiExtensions
{
    public static IWorldSpawnBuilder SetMobAiModifier(this IWorldSpawnBuilder builder, string aiName, string mobAiConfig)
    {
        if (IntegrationManager.InstalledMobAI)
        {
            builder.AddOrReplaceModifier(new ModifierSetAI(aiName, mobAiConfig));
        }

        return builder;
    }
}
