using Valheim.SpawnThat.Integrations;
using Valheim.SpawnThat.Integrations.CLLC.Modifiers;
using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;

namespace Valheim.SpawnThat.Spawners;

public static class ILocalSpawnBuilderCllcModifierExtensions
{
    public static ILocalSpawnBuilder SetCllcModifierBossAffix(this ILocalSpawnBuilder builder, string bossAffixName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.AddOrReplaceModifier(new ModifierCllcBossAffix(bossAffixName));
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierExtraEffect(this ILocalSpawnBuilder builder, string extraEffectName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.AddOrReplaceModifier(new ModifierCllcExtraEffect(extraEffectName));
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierInfusion(this ILocalSpawnBuilder builder, string infusionName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.AddOrReplaceModifier(new ModifierCllcInfusion(infusionName));
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierRandomLevel(this ILocalSpawnBuilder builder)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.AddOrReplaceModifier(new ModifierCllcRandomLevel());
        }

        return builder;
    }
}
