using System.Collections.Generic;
using System.Linq;
using SpawnThat.Options.Conditions;
using SpawnThat.Utilities.Enums;

namespace SpawnThat.Spawners;

public static class IHaveSpawnConditionsExtensions
{
    /// <summary>
    /// <para>Set area ids in which spawning is enabled.</para>
    /// <para>Should only be used when map is pre-determined.</para>
    /// </summary>
    public static T SetConditionAreaIds<T>(this T builder, IEnumerable<int> areaIds)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionAreaIds(areaIds.ToList()));
        return builder;
    }

    /// <summary>
    /// <para>Set area ids in which spawning is enabled.</para>
    /// <para>Should only be used when map is pre-determined.</para>
    /// </summary>
    public static T SetConditionAreaIds<T>(this T builder, params int[] areaIds)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionAreaIds(areaIds.ToList()));
        return builder;
    }

    /// <summary>
    /// <para>Set spawn chance based on each area.</para>
    /// <para>
    ///     Each area rolls chance pr template id once pr seed.
    ///     If the chance roll is less than listed here,
    ///     this area will never activate this template, and vice versa.
    /// </para>
    /// <para>
    ///     This allows for sitations where only some areas (eg., 10% of blackforests) will have a spawn show up.
    /// </para>
    /// </summary>
    public static T SetConditionAreaSpawnChance<T>(this T builder, float areaChance, int templateId)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionAreaSpawnChance(areaChance, templateId));
        return builder;
    }

    /// <summary>
    /// <para>Spawn template is only active when player is within <c>withinDistance</c> to Spawner.</para>
    /// </summary>
    public static T SetConditionCloseToPlayer<T>(this T builder, float withinDistance)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionCloseToPlayer(withinDistance));
        return builder;
    }

    /// <summary>
    /// Sets range from center of world, in which spawning is allowed.
    /// </summary>
    /// <param name="minDistance">Required distance from center.</param>
    /// <param name="maxDistance">Maximum distance from center. If null, there will be no limit.</param>
    public static T SetConditionDistanceToCenter<T>(this T builder, double? minDistance = null, double? maxDistance = null)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionDistanceToCenter(minDistance, maxDistance));
        return builder;
    }

    /// <summary>
    /// <para>Sets global keys which must not be present for spawning to be allowed.</para>
    /// <para>
    ///     Eg., if "KilledTroll", "defeated_eikthyr" and world has global key "defeated_eikthyr",
    ///     then this template will be disabled.
    /// </para>
    /// </summary>
    public static T SetGlobalKeysRequiredMissing<T>(this T builder, IEnumerable<string> globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysRequiredMissing(globalKeys.ToArray()));
        return builder;
    }

    /// <summary>
    /// <para>Sets global keys which must not be present for spawning to be allowed.</para>
    /// <para>
    ///     Eg., if "KilledTroll", "defeated_eikthyr" and world has global key "defeated_eikthyr",
    ///     then this template will be disabled.
    /// </para>
    /// </summary>
    public static T SetGlobalKeysRequiredMissing<T>(this T builder, params string[] globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysRequiredMissing(globalKeys));
        return builder;
    }

    /// <summary>
    /// <para>Sets global keys which must not be present for spawning to be allowed.</para>
    /// <para>
    ///     Eg., if GlobalKey.Troll, GlobalKey.Eikthyr and world has global key "defeated_eikthyr",
    ///     then this template will be disabled.
    /// </para>
    /// </summary>
    public static T SetGlobalKeysRequiredMissing<T>(this T builder, IEnumerable<GlobalKey> globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysRequiredMissing(globalKeys.Select(x => x.GetName()).ToArray()));
        return builder;
    }

    /// <summary>
    /// <para>Sets global keys which must not be present for spawning to be allowed.</para>
    /// <para>
    ///     Eg., if GlobalKey.Troll, GlobalKey.Eikthyr and world has global key "defeated_eikthyr",
    ///     then this template will be disabled.
    /// </para>
    /// </summary>
    public static T SetGlobalKeysRequiredMissing<T>(this T builder, params GlobalKey[] globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysRequiredMissing(globalKeys.Select(x => x.GetName()).ToArray()));
        return builder;
    }

    /// <summary>
    /// Sets locations in which spawning is allowed.
    /// </summary>
    public static T SetConditionLocation<T>(this T builder, IEnumerable<string> locationNames)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionLocation(locationNames.ToArray()));
        return builder;
    }

    /// <summary>
    /// Sets locations in which spawning is allowed.
    /// </summary>
    public static T SetConditionLocation<T>(this T builder, params string[] locationNames)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionLocation(locationNames));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has one of the listed items in inventory.
    /// </summary>
    public static T SetConditionNearbyPlayersCarryItem<T>(this T builder, int distance, IEnumerable<string> itemPrefabNames)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionNearbyPlayersCarryItem(distance, itemPrefabNames.ToArray()));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has one of the listed items in inventory.
    /// </summary>
    public static T SetConditionNearbyPlayersCarryItem<T>(this T builder, int distance, params string[] itemPrefabNames)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionNearbyPlayersCarryItem(distance, itemPrefabNames));
        return builder;
    }

    /// <summary>
    /// Template only enabled when players within <c>distance</c> of spawner has a combined inventory value greater than <c>combinedValueRequired</c>.
    /// </summary>
    public static T SetConditionNearbyPlayersCarryValue<T>(this T builder, int distance, int combinedValueRequired)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionNearbyPlayersCarryValue(distance, combinedValueRequired));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner emits more noise than <c>noiseRequired</c>.
    /// </summary>
    public static T SetConditionNearbyPlayersNoise<T>(this T builder, int distance, float noiseRequired)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionNearbyPlayersNoise(distance, noiseRequired));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has a status in <c>statusEffectNames</c>.
    /// </summary>
    public static T SetConditionNearbyPlayersStatus<T>(this T builder, int distance, IEnumerable<string> statusEffectNames)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionNearbyPlayersStatus(distance, statusEffectNames.ToArray()));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has a status in <c>statusEffectNames</c>.
    /// </summary>
    public static T SetConditionNearbyPlayersStatus<T>(this T builder, int distance, params string[] statusEffectNames)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionNearbyPlayersStatus(distance, statusEffectNames));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has a status in <c>statusEffects</c>.
    /// </summary>
    public static T SetConditionNearbyPlayersStatus<T>(this T builder, int distance, IEnumerable<StatusEffect> statusEffects)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionNearbyPlayersStatus(distance, statusEffects.Select(x => x.ToString()).ToArray()));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has a status in <c>statusEffects</c>.
    /// </summary>
    public static T SetConditionNearbyPlayersStatus<T>(this T builder, int distance, params StatusEffect[] statusEffects)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionNearbyPlayersStatus(distance, statusEffects.Select(x => x.ToString()).ToArray()));
        return builder;
    }

    /// <summary>
    /// Template only enabled when the world day is within the indicated range.
    /// </summary>
    /// <param name="minDays">Minimum days required before template is activated.</param>
    /// <param name="maxDays">Maximum days after which template is deactivated. If null, there is no limit.</param>
    public static T SetConditionWorldAge<T>(this T builder, int? minDays = null, int? maxDays = null)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionWorldAge(minDays, maxDays));
        return builder;
    }
}
