using System.Collections.Generic;
using SpawnThat.Options.Modifiers;

namespace SpawnThat.Spawners.LocalSpawner;

public static class ILocalSpawnBuilderModifierExtensions
{
    public static ILocalSpawnBuilder SetModifierDespawnOnAlert(this ILocalSpawnBuilder builder, bool despawnOnAlert)
    {
        builder.SetModifier(new ModifierDespawnOnAlert(despawnOnAlert));
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierDespawnOnConditionsInvalid(
        this ILocalSpawnBuilder builder,
        bool? conditionAllowDuringDay = null,
        bool? conditionAllowDuringNight = null,
        List<string> conditionAllowDuringEnvironments = null)
    {
        builder.SetModifier(
            new ModifierDespawnOnConditionsInvalid(
                conditionAllowDuringDay,
                conditionAllowDuringNight,
                conditionAllowDuringEnvironments));
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierRelentless(this ILocalSpawnBuilder builder, bool relentless)
    {
        builder.SetModifier(new ModifierSetRelentless(relentless));
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierFaction(this ILocalSpawnBuilder builder, Character.Faction faction)
    {
        builder.SetModifier(new ModifierSetFaction(faction));
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierTamed(this ILocalSpawnBuilder builder, bool tamed)
    {
        builder.SetModifier(new ModifierSetTamed(tamed));
        return builder;
    }

    public static ILocalSpawnBuilder SetModifierTamedCommandable(this ILocalSpawnBuilder builder, bool commandable)
    {
        builder.SetModifier(new ModifierSetTamedCommandable(commandable));
        return builder;
    }

    /// <summary>
    /// Sets a custom string on entity zdo with key "spawn_template_id".
    /// Intended for integration between mods, and detecting a modified entity.
    /// </summary>
    public static ILocalSpawnBuilder SetModifierTemplateId(this ILocalSpawnBuilder builder, string templateId)
    {
        builder.SetModifier(new ModifierSetTemplateId(templateId));
        return builder;
    }
}
