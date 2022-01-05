using Valheim.SpawnThat.Integrations;
using Valheim.SpawnThat.Integrations.MobAi.Modifiers;
using Valheim.SpawnThat.Spawners.WorldSpawner;

namespace Valheim.SpawnThat.Spawners;

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
