using System.Collections.Generic;
using SpawnThat.Options.Modifiers;
using SpawnThat.Spawners.LocalSpawner.Configuration;

namespace SpawnThat.Spawners;

public static class ILocalSpawnBuilderModifierExtensions
{
    public static ILocalSpawnBuilder SetModifierDespawnOnAlert(this ILocalSpawnBuilder builder, bool despawnOnAlert)
    {
        builder.AddOrReplaceModifier(new ModifierDespawnOnAlert(despawnOnAlert));
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

    public static ILocalSpawnBuilder SetModifierRelentless(this ILocalSpawnBuilder builder, bool relentless)
    {
        builder.AddOrReplaceModifier(new ModifierSetRelentless(relentless));
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierFaction(this ILocalSpawnBuilder builder, Character.Faction faction)
    {
        builder.AddOrReplaceModifier(new ModifierSetFaction(faction));
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierTamed(this ILocalSpawnBuilder builder, bool tamed)
    {
        builder.AddOrReplaceModifier(new ModifierSetTamed(tamed));
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierTamedCommandable(this ILocalSpawnBuilder builder, bool commandable)
    {
        builder.AddOrReplaceModifier(new ModifierSetTamedCommandable(commandable));
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierTemplateId(this ILocalSpawnBuilder builder, string templateId)
    {
        builder.AddOrReplaceModifier(new ModifierSetTemplateId(templateId));
        return builder;
    }
}
