using SpawnThat.Integrations;
using SpawnThat.Integrations.CLLC.Models;
using SpawnThat.Integrations.CLLC.Modifiers;

namespace SpawnThat.Spawners;

public static class IHaveSpawnModifiersCllcModifierExtensions
{
    public static T SetCllcModifierBossAffix<T>(this T builder, string bossAffixName)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledCLLC)
{
            SetBossAffix(builder, bossAffixName);
        }

        return builder;
    }

    public static T SetCllcModifierBossAffix<T>(this T builder, CllcBossAffix bossAffix)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetBossAffix(builder, bossAffix);
        }

        return builder;
    }

    private static void SetBossAffix(IHaveSpawnModifiers builder, string bossAffixName) => builder.SetModifier(new ModifierCllcBossAffix(bossAffixName));
    private static void SetBossAffix(IHaveSpawnModifiers builder, CllcBossAffix bossAffix) => builder.SetModifier(new ModifierCllcBossAffix(bossAffix));

    public static T SetCllcModifierExtraEffect<T>(this T builder, string extraEffectName)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetExtraEffect(builder, extraEffectName);
        }

        return builder;
    }

    public static T SetCllcModifierExtraEffect<T>(this T builder, CllcCreatureExtraEffect extraEffect)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetExtraEffect(builder, extraEffect);
        }

        return builder;
    }

    private static void SetExtraEffect(IHaveSpawnModifiers builder, string extraEffectName) => builder.SetModifier(new ModifierCllcExtraEffect(extraEffectName));
    private static void SetExtraEffect(IHaveSpawnModifiers builder, CllcCreatureExtraEffect extraEffect) => builder.SetModifier(new ModifierCllcExtraEffect(extraEffect));
    
    public static T SetCllcModifierInfusion<T>(this T builder, string infusionName)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetInfusion(builder, infusionName);
        }

        return builder;
    }

    public static T SetCllcModifierInfusion<T>(this T builder, CllcCreatureInfusion infusion)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetInfusion(builder, infusion);
        }

        return builder;
    }

    private static void SetInfusion(IHaveSpawnModifiers builder, string infusionName) => builder.SetModifier(new ModifierCllcInfusion(infusionName));
    private static void SetInfusion(IHaveSpawnModifiers builder, CllcCreatureInfusion infusion) => builder.SetModifier(new ModifierCllcInfusion(infusion));

    public static T SetCllcModifierRandomLevel<T>(this T builder)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledCLLC)
        {
            SetRandomLevel(builder);
        }

        return builder;
    }

    private static void SetRandomLevel(IHaveSpawnModifiers builder) => builder.SetModifier(new ModifierCllcRandomLevel());
}
