using Valheim.SpawnThat.Integrations;
using Valheim.SpawnThat.Integrations.CLLC.Modifiers;
using Valheim.SpawnThat.Spawners.WorldSpawner;

namespace Valheim.SpawnThat.Spawners;

public static class IWorldSpawnBuilderCllcModifierExtensions
{
    public static IWorldSpawnBuilder SetCllcModifierBossAffix(this IWorldSpawnBuilder builder, string bossAffixName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.AddOrReplaceModifier(new ModifierCllcBossAffix(bossAffixName));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierExtraEffect(this IWorldSpawnBuilder builder, string extraEffectName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.AddOrReplaceModifier(new ModifierCllcExtraEffect(extraEffectName));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierInfusion(this IWorldSpawnBuilder builder, string infusionName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.AddOrReplaceModifier(new ModifierCllcInfusion(infusionName));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierRandomLevel(this IWorldSpawnBuilder builder)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.AddOrReplaceModifier(new ModifierCllcRandomLevel());
        }

        return builder;
    }
}
