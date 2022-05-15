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
            SetBossAffix(builder, bossAffixName);
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierBossAffix(this ILocalSpawnBuilder builder, CllcBossAffix bossAffix)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetBossAffix(builder, bossAffix);
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierBossAffix(this ILocalSpawnBuilder builder, CllcBossAffix? bossAffix)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetBossAffix(builder, bossAffix);
        }

        return builder;
    }

    private static void SetBossAffix(ILocalSpawnBuilder builder, string bossAffixName) => builder.SetModifier(new ModifierCllcBossAffix(bossAffixName));
    private static void SetBossAffix(ILocalSpawnBuilder builder, CllcBossAffix? bossAffix) => builder.SetModifier(new ModifierCllcBossAffix(bossAffix));

    public static ILocalSpawnBuilder SetCllcModifierExtraEffect(this ILocalSpawnBuilder builder, string extraEffectName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetExtraEffect(builder, extraEffectName);
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierExtraEffect(this ILocalSpawnBuilder builder, CllcCreatureExtraEffect extraEffect)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetExtraEffect(builder, extraEffect);
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierExtraEffect(this ILocalSpawnBuilder builder, CllcCreatureExtraEffect? extraEffect)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetExtraEffect(builder, extraEffect);
        }

        return builder;
    }

    private static void SetExtraEffect(ILocalSpawnBuilder builder, string extraEffectName) => builder.SetModifier(new ModifierCllcExtraEffect(extraEffectName));
    private static void SetExtraEffect(ILocalSpawnBuilder builder, CllcCreatureExtraEffect? extraEffect) => builder.SetModifier(new ModifierCllcExtraEffect(extraEffect));

    public static ILocalSpawnBuilder SetCllcModifierInfusion(this ILocalSpawnBuilder builder, string infusionName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetInfusion(builder, infusionName);
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierInfusion(this ILocalSpawnBuilder builder, CllcCreatureInfusion infusion)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetInfusion(builder, infusion);
        }

        return builder;
    }

    public static ILocalSpawnBuilder SetCllcModifierInfusion(this ILocalSpawnBuilder builder, CllcCreatureInfusion? infusion)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetInfusion(builder, infusion);
        }

        return builder;
    }

    private static void SetInfusion(ILocalSpawnBuilder builder, string infusionName) => builder.SetModifier(new ModifierCllcInfusion(infusionName));
    private static void SetInfusion(ILocalSpawnBuilder builder, CllcCreatureInfusion? infusion) => builder.SetModifier(new ModifierCllcInfusion(infusion));


    public static ILocalSpawnBuilder SetCllcModifierRandomLevel(this ILocalSpawnBuilder builder)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetRandomLevel(builder);
        }

        return builder;
    }

    private static void SetRandomLevel(ILocalSpawnBuilder builder) => builder.SetModifier(new ModifierCllcRandomLevel());
}
