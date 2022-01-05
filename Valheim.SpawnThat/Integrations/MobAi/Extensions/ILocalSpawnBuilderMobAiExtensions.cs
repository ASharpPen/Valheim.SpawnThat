using Valheim.SpawnThat.Integrations.MobAi.Modifiers;
using Valheim.SpawnThat.Integrations;
using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;

namespace Valheim.SpawnThat.Spawners;

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
