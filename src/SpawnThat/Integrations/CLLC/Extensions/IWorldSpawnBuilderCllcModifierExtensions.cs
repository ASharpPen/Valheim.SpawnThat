using SpawnThat.Integrations;
using SpawnThat.Integrations.CLLC.Models;
using SpawnThat.Integrations.CLLC.Modifiers;

namespace SpawnThat.Spawners.WorldSpawner;

public static class IWorldSpawnBuilderCllcModifierExtensions
{
    public static IWorldSpawnBuilder SetCllcModifierBossAffix(this IWorldSpawnBuilder builder, string bossAffixName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetBossAffix(builder, bossAffixName);
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierBossAffix(this IWorldSpawnBuilder builder, CllcBossAffix bossAffix)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetBossAffix(builder, bossAffix);
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierBossAffix(this IWorldSpawnBuilder builder, CllcBossAffix? bossAffix)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetBossAffix(builder, bossAffix);
        }

        return builder;
    }

    private static void SetBossAffix(IWorldSpawnBuilder builder, string bossAffixName) => builder.SetModifier(new ModifierCllcBossAffix(bossAffixName));
    private static void SetBossAffix(IWorldSpawnBuilder builder, CllcBossAffix? bossAffix) => builder.SetModifier(new ModifierCllcBossAffix(bossAffix));

    public static IWorldSpawnBuilder SetCllcModifierExtraEffect(this IWorldSpawnBuilder builder, string extraEffect)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetExtraEffect(builder, extraEffect);
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierExtraEffect(this IWorldSpawnBuilder builder, CllcCreatureExtraEffect extraEffect)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetExtraEffect(builder, extraEffect);
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierExtraEffect(this IWorldSpawnBuilder builder, CllcCreatureExtraEffect? extraEffect)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetExtraEffect(builder, extraEffect);
        }

        return builder;
    }

    private static void SetExtraEffect(IWorldSpawnBuilder builder, string extraEffectName) => builder.SetModifier(new ModifierCllcExtraEffect(extraEffectName));
    private static void SetExtraEffect(IWorldSpawnBuilder builder, CllcCreatureExtraEffect? extraEffect) => builder.SetModifier(new ModifierCllcExtraEffect(extraEffect));

    public static IWorldSpawnBuilder SetCllcModifierInfusion(this IWorldSpawnBuilder builder, string infusion)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetInfusion(builder, infusion);
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierInfusion(this IWorldSpawnBuilder builder, CllcCreatureInfusion infusion)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetInfusion(builder, infusion);
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierInfusion(this IWorldSpawnBuilder builder, CllcCreatureInfusion? infusion)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetInfusion(builder, infusion);
        }

        return builder;
    }

    private static void SetInfusion(IWorldSpawnBuilder builder, string infusionName) => builder.SetModifier(new ModifierCllcInfusion(infusionName));
    private static void SetInfusion(IWorldSpawnBuilder builder, CllcCreatureInfusion? infusion) => builder.SetModifier(new ModifierCllcInfusion(infusion));

    public static IWorldSpawnBuilder SetCllcModifierRandomLevel(this IWorldSpawnBuilder builder)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetRandomLevel(builder);
        }

        return builder;
    }

    private static void SetRandomLevel(IWorldSpawnBuilder builder) => builder.SetModifier(new ModifierCllcRandomLevel());
}
