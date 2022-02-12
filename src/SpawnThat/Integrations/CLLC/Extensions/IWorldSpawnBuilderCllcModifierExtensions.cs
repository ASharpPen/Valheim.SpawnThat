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
            builder.SetModifier(new ModifierCllcBossAffix(bossAffixName));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierBossAffix(this IWorldSpawnBuilder builder, CllcBossAffix bossAffix)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcBossAffix(bossAffix));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierExtraEffect(this IWorldSpawnBuilder builder, CllcCreatureExtraEffect extraEffect)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcExtraEffect(extraEffect));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierInfusion(this IWorldSpawnBuilder builder, CllcCreatureInfusion infusion)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcInfusion(infusion));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierRandomLevel(this IWorldSpawnBuilder builder)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcRandomLevel());
        }

        return builder;
    }
}
