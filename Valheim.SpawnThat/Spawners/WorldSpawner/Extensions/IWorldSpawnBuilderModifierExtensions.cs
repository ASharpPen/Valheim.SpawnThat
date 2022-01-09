using System.Collections.Generic;
using Valheim.SpawnThat.Spawn.Modifiers;
using Valheim.SpawnThat.Spawners.WorldSpawner;

namespace Valheim.SpawnThat.Spawners;

public static class IWorldSpawnBuilderModifierExtensions
{
    public static IWorldSpawnBuilder SetModifierDespawnOnAlert(this IWorldSpawnBuilder builder, bool despawnOnAlert)
    {
        builder.AddOrReplaceModifier(new ModifierDespawnOnAlert(despawnOnAlert));
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

    public static IWorldSpawnBuilder SetModifierRelentless(this IWorldSpawnBuilder builder, bool relentless)
    {
        builder.AddOrReplaceModifier(new ModifierSetRelentless(relentless));
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierFaction(this IWorldSpawnBuilder builder, Character.Faction faction)
    {
        builder.AddOrReplaceModifier(new ModifierSetFaction(faction));
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierTamed(this IWorldSpawnBuilder builder, bool tamed = true)
    {
        builder.AddOrReplaceModifier(new ModifierSetTamed(tamed));
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierTamedCommandable(this IWorldSpawnBuilder builder, bool commandable)
    {
        builder.AddOrReplaceModifier(new ModifierSetTamedCommandable(commandable));
        return builder;
    }

    public static IWorldSpawnBuilder SetModifierTemplateId(this IWorldSpawnBuilder builder, string templateId)
    {
        builder.AddOrReplaceModifier(new ModifierSetTemplateId(templateId));
        return builder;
    }
}
