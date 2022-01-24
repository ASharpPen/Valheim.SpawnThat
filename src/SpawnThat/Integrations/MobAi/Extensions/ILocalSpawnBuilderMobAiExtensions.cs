using SpawnThat.Integrations.MobAi.Modifiers;
using SpawnThat.Integrations;
using SpawnThat.Spawners.LocalSpawner.Configuration;

namespace SpawnThat.Spawners;

public static class ILocalSpawnBuilderMobAiExtensions
{
    public static ILocalSpawnBuilder SetMobAiModifier(this ILocalSpawnBuilder builder, string aiName, string mobAiConfig)
    {
        if (IntegrationManager.InstalledMobAI)
        {
            builder.AddOrReplaceModifier(new ModifierSetAI(aiName, mobAiConfig));
        }

        return builder;
    }
}
