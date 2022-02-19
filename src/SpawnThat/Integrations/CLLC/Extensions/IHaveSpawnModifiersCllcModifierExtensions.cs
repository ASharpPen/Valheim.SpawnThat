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
            builder.SetModifier(new ModifierCllcBossAffix(bossAffixName));
        }

        return builder;
    }

    public static T SetCllcModifierBossAffix<T>(this T builder, CllcBossAffix bossAffix)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcBossAffix(bossAffix));
        }

        return builder;
    }

    public static T SetCllcModifierExtraEffect<T>(this T builder, CllcCreatureExtraEffect extraEffect)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcExtraEffect(extraEffect));
        }

        return builder;
    }

    public static T SetCllcModifierInfusion<T>(this T builder, CllcCreatureInfusion infusion)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcInfusion(infusion));
        }

        return builder;
    }

    public static T SetCllcModifierRandomLevel<T>(this T builder)
        where T : IHaveSpawnModifiers
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcRandomLevel());
        }

        return builder;
    }
}
