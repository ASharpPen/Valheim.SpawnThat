using SpawnThat.Options.Modifiers;
using System.Collections.Generic;

namespace SpawnThat.Spawners;

public static class IHaveSpawnModifiersExtensions
{
    public static T SetModifierDespawnOnAlert<T>(this T builder, bool despawnOnAlert)
        where T : IHaveSpawnModifiers
    {
        builder.SetModifier(new ModifierDespawnOnAlert(despawnOnAlert));
        return builder;
    }

    public static T SetModifierDespawnOnConditionsInvalid<T>(
        this T builder,
        bool? conditionAllowDuringDay = null,
        bool? conditionAllowDuringNight = null,
        List<string> conditionAllowDuringEnvironments = null)
        where T : IHaveSpawnModifiers
    {
        builder.SetModifier(
            new ModifierDespawnOnConditionsInvalid(
                conditionAllowDuringDay,
                conditionAllowDuringNight,
                conditionAllowDuringEnvironments));
        return builder;
    }

    public static T SetModifierRelentless<T>(this T builder, bool relentless)
        where T : IHaveSpawnModifiers
    {
        builder.SetModifier(new ModifierSetRelentless(relentless));
        return builder;
    }

    public static T SetModifierFaction<T>(this T builder, Character.Faction faction)
        where T : IHaveSpawnModifiers
    {
        builder.SetModifier(new ModifierSetFaction(faction));
        return builder;
    }

    public static T SetModifierFaction<T>(this T builder, string faction)
        where T : IHaveSpawnModifiers
    {
        builder.SetModifier(new ModifierSetFaction(faction));
        return builder;
    }

    public static T SetModifierHuntPlayer<T>(this T builder, bool huntPlayer)
        where T : IHaveSpawnModifiers
    {
        builder.SetModifier(new ModifierSetHuntPlayer(huntPlayer));
        return builder;
    }

    public static T SetModifierTamed<T>(this T builder, bool tamed = true)
        where T : IHaveSpawnModifiers
    {
        builder.SetModifier(new ModifierSetTamed(tamed));
        return builder;
    }

    public static T SetModifierTamedCommandable<T>(this T builder, bool commandable)
        where T : IHaveSpawnModifiers
    {
        builder.SetModifier(new ModifierSetTamedCommandable(commandable));
        return builder;
    }

    /// <summary>
    /// Sets a custom string on entity zdo with key "spawn_template_id".
    /// Intended for integration between mods, and detecting a modified entity.
    /// </summary>
    public static T SetModifierTemplateId<T>(this T builder, string templateId)
        where T : IHaveSpawnModifiers
    {
        builder.SetModifier(new ModifierSetTemplateId(templateId));
        return builder;
    }
}
