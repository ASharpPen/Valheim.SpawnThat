using SpawnThat.Integrations;
using SpawnThat.Integrations.CLLC.Modifiers;
using SpawnThat.Spawners.WorldSpawner;

namespace SpawnThat.Spawners;

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

    public static IWorldSpawnBuilder SetCllcModifierExtraEffect(this IWorldSpawnBuilder builder, string extraEffectName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcExtraEffect(extraEffectName));
        }

        return builder;
    }

    public static IWorldSpawnBuilder SetCllcModifierInfusion(this IWorldSpawnBuilder builder, string infusionName)
    {
        if (IntegrationManager.InstalledCLLC)
        {
            builder.SetModifier(new ModifierCllcInfusion(infusionName));
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
