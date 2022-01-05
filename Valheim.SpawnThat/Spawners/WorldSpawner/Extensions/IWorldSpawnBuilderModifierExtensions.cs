using System.Collections.Generic;
using Valheim.SpawnThat.Spawn.Modifiers;
using Valheim.SpawnThat.Spawners.WorldSpawner;

namespace Valheim.SpawnThat.Spawners;

public static class IWorldSpawnBuilderModifierExtensions
{
    public static IWorldSpawnBuilder SetModifierDespawnOnAlert(this IWorldSpawnBuilder builder)
    {
        builder.AddOrReplaceModifier(new ModifierDespawnOnAlert());
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierDespawnOnConditionsInvalid(
        this IWorldSpawnBuilder builder,
        bool? conditionAllowDuringDay = null,
        bool? conditionAllowDuringNight = null,
        List<string> conditionAllowDuringEnvironments = null)
    {
        builder.AddOrReplaceModifier(
            new ModifierDespawnOnConditionsInvalid(
                conditionAllowDuringDay,
                conditionAllowDuringNight,
                conditionAllowDuringEnvironments));
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierRelentless(this IWorldSpawnBuilder builder)
    {
        builder.AddOrReplaceModifier(new ModifierRelentless());
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierFaction(this IWorldSpawnBuilder builder, Character.Faction faction)
    {
        builder.AddOrReplaceModifier(new ModifierSetFaction(faction));
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierTamed(this IWorldSpawnBuilder builder)
    {
        builder.AddOrReplaceModifier(new ModifierSetTamed());
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierTamedCommandable(this IWorldSpawnBuilder builder)
    {
        builder.AddOrReplaceModifier(new ModifierSetTamedCommandable());
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierTemplateId(this IWorldSpawnBuilder builder, string templateId)
    {
        builder.AddOrReplaceModifier(new ModifierSetTemplateId(templateId));
        return builder;
    }
}
