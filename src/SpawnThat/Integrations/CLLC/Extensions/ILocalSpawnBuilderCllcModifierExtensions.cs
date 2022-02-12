using SpawnThat.Integrations;
using SpawnThat.Integrations.CLLC.Models;
using SpawnThat.Integrations.CLLC.Modifiers;

namespace SpawnThat.Spawners.LocalSpawner;

public static class ILocalSpawnBuilderCllcModifierExtensions
{
    public static ILocalSpawnBuilder SetCllcModifierBossAffix(this ILocalSpawnBuilder builder, string bossAffixName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcBossAffix(bossAffixName));
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierBossAffix(this ILocalSpawnBuilder builder, CllcBossAffix bossAffix)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcBossAffix(bossAffix));
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierExtraEffect(this ILocalSpawnBuilder builder, string extraEffectName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcExtraEffect(extraEffectName));
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierExtraEffect(this ILocalSpawnBuilder builder, CllcCreatureExtraEffect extraEffect)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcExtraEffect(extraEffect));
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierInfusion(this ILocalSpawnBuilder builder, string infusionName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcInfusion(infusionName));
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierInfusion(this ILocalSpawnBuilder builder, CllcCreatureInfusion infusion)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcInfusion(infusion));
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierRandomLevel(this ILocalSpawnBuilder builder)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcRandomLevel());
        }

        return builder;
    }
}
