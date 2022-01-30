using System.Collections.Generic;
using SpawnThat.Options.Modifiers;
using SpawnThat.Spawners.WorldSpawner;

namespace SpawnThat.Spawners;

public static class IWorldSpawnBuilderModifierExtensions
{
    public static IWorldSpawnBuilder SetModifierDespawnOnAlert(this IWorldSpawnBuilder builder, bool despawnOnAlert)
    {
        builder.SetModifier(new ModifierDespawnOnAlert(despawnOnAlert));
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierDespawnOnConditionsInvalid(
        this IWorldSpawnBuilder builder,
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

    public static IWorldSpawnBuilder SetModifierRelentless(this IWorldSpawnBuilder builder, bool relentless)
    {
        builder.SetModifier(new ModifierSetRelentless(relentless));
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierFaction(this IWorldSpawnBuilder builder, Character.Faction faction)
    {
        builder.SetModifier(new ModifierSetFaction(faction));
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierTamed(this IWorldSpawnBuilder builder, bool tamed = true)
    {
        builder.SetModifier(new ModifierSetTamed(tamed));
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierTamedCommandable(this IWorldSpawnBuilder builder, bool commandable)
    {
        builder.SetModifier(new ModifierSetTamedCommandable(commandable));
        return builder;
    }

    /// <summary>
    /// Sets a custom id on entity zdo as "spawn_template_id".
    /// Intended for integration between mods, and detecting a modified entity.
    /// </summary>
    public static IWorldSpawnBuilder SetModifierTemplateId(this IWorldSpawnBuilder builder, string templateId)
    {
        builder.SetModifier(new ModifierSetTemplateId(templateId));
        return builder;
    }
}
