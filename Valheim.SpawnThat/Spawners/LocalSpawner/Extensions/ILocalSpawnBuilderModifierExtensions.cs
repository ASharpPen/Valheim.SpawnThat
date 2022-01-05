using System.Collections.Generic;
using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;
using Valheim.SpawnThat.Spawn.Modifiers;

namespace Valheim.SpawnThat.Spawners;

public static class ILocalSpawnBuilderModifierExtensions
{
    public static ILocalSpawnBuilder SetModifierDespawnOnAlert(this ILocalSpawnBuilder builder)
    {
        builder.AddOrReplaceModifier(new ModifierDespawnOnAlert());
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierDespawnOnConditionsInvalid(
        this ILocalSpawnBuilder builder,
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

    public static ILocalSpawnBuilder SetModifierRelentless(this ILocalSpawnBuilder builder)
    {
        builder.AddOrReplaceModifier(new ModifierRelentless());
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierFaction(this ILocalSpawnBuilder builder, Character.Faction faction)
    {
        builder.AddOrReplaceModifier(new ModifierSetFaction(faction));
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierTamed(this ILocalSpawnBuilder builder)
    {
        builder.AddOrReplaceModifier(new ModifierSetTamed());
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierTamedCommandable(this ILocalSpawnBuilder builder)
    {
        builder.AddOrReplaceModifier(new ModifierSetTamedCommandable());
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierTemplateId(this ILocalSpawnBuilder builder, string templateId)
    {
        builder.AddOrReplaceModifier(new ModifierSetTemplateId(templateId));
        return builder;
    }
}
