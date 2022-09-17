using System.Collections.Generic;
using System.Linq;
using SpawnThat.Options.Conditions;
using SpawnThat.Utilities.Enums;

namespace SpawnThat.Spawners.LocalSpawner;

public static class ILocalSpawnBuilderConditionExtensions
{    
    /// <summary>
    /// <para>Set area ids in which spawning is enabled.</para>
    /// <para>Should only be used when map is pre-determined.</para>
    /// </summary>
    public static ILocalSpawnBuilder SetConditionAreaIds(this ILocalSpawnBuilder builder, IEnumerable<int> areaIds)
    {
        builder.SetCondition(new ConditionAreaIds(areaIds.ToList()));
        return builder;
    }

    /// <summary>
    /// <para>Set area ids in which spawning is enabled.</para>
    /// <para>Should only be used when map is pre-determined.</para>
    /// </summary>
    public static ILocalSpawnBuilder SetConditionAreaIds(this ILocalSpawnBuilder builder, params int[] areaIds)
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
    public static ILocalSpawnBuilder SetConditionAreaSpawnChance(this ILocalSpawnBuilder builder, float areaChance, uint templateId)
    {
        builder.SetCondition(new ConditionAreaSpawnChance(areaChance, (int)templateId));
        return builder;
    }

    /// <summary>
    /// <para>Spawn template is only active when player is within <c>withinDistance</c> to Spawner.</para>
    /// </summary>
    public static ILocalSpawnBuilder SetConditionCloseToPlayer(this ILocalSpawnBuilder builder, float withinDistance)
    {
        builder.SetCondition(new ConditionCloseToPlayer(withinDistance));
        return builder;
    }

    /// <summary>
    /// Sets range from center of world, in which spawning is allowed.
    /// </summary>
    /// <param name="minDistance">Required distance from center.</param>
    /// <param name="maxDistance">Maximum distance from center. If null, there will be no limit.</param>
    public static ILocalSpawnBuilder SetConditionDistanceToCenter(this ILocalSpawnBuilder builder, double? minDistance = null, double? maxDistance = null)
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
    public static ILocalSpawnBuilder SetGlobalKeysRequiredMissing(this ILocalSpawnBuilder builder, IEnumerable<string> globalKeys)
    {
        builder.SetCondition(new ConditionGlobalKeysRequiredMissing(globalKeys?.ToArray()));
        return builder;
    }

    /// <summary>
    /// <para>Sets global keys which must not be present for spawning to be allowed.</para>
    /// <para>
    ///     Eg., if "KilledTroll", "defeated_eikthyr" and world has global key "defeated_eikthyr",
    ///     then this template will be disabled.
    /// </para>
    /// </summary>
    public static ILocalSpawnBuilder SetGlobalKeysRequiredMissing(this ILocalSpawnBuilder builder, params string[] globalKeys)
    {
        builder.SetCondition(new ConditionGlobalKeysRequiredMissing(globalKeys?.ToArray()));
        return builder;
    }

    /// <summary>
    /// <para>Sets global keys which must not be present for spawning to be allowed.</para>
    /// <para>
    ///     Eg., if GlobalKey.Troll, GlobalKey.Eikthyr and world has global key "defeated_eikthyr",
    ///     then this template will be disabled.
    /// </para>
    /// </summary>
    public static ILocalSpawnBuilder SetGlobalKeysRequiredMissing(this ILocalSpawnBuilder builder, IEnumerable<GlobalKey> globalKeys)
    {
        builder.SetCondition(new ConditionGlobalKeysRequiredMissing(globalKeys?.Select(x => x.GetName()).ToArray()));
        return builder;
    }

    /// <summary>
    /// <para>Sets global keys which must not be present for spawning to be allowed.</para>
    /// <para>
    ///     Eg., if GlobalKey.Troll, GlobalKey.Eikthyr and world has global key "defeated_eikthyr",
    ///     then this template will be disabled.
    /// </para>
    /// </summary>
    public static ILocalSpawnBuilder SetGlobalKeysRequiredMissing(this ILocalSpawnBuilder builder, params GlobalKey[] globalKeys)
    {
        builder.SetCondition(new ConditionGlobalKeysRequiredMissing(globalKeys?.Select(x => x.GetName()).ToArray()));
        return builder;
    }

    /// <summary>
    /// Sets locations in which spawning is allowed.
    /// </summary>
    public static ILocalSpawnBuilder SetConditionLocation(this ILocalSpawnBuilder builder, IEnumerable<string> locationNames)
    {
        builder.SetCondition(new ConditionLocation(locationNames?.ToArray()));
        return builder;
    }

    /// <summary>
    /// Sets locations in which spawning is allowed.
    /// </summary>
    public static ILocalSpawnBuilder SetConditionLocation(this ILocalSpawnBuilder builder, params string[] locationNames)
    {
        builder.SetCondition(new ConditionLocation(locationNames));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has one of the listed items in inventory.
    /// </summary>
    public static ILocalSpawnBuilder AddConditionNearbyPlayersCarryItem(this ILocalSpawnBuilder builder, int distance, IEnumerable<string> itemPrefabNames)
    {
        builder.SetCondition(new ConditionNearbyPlayersCarryItem(distance, itemPrefabNames?.ToArray()));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has one of the listed items in inventory.
    /// </summary>
    public static ILocalSpawnBuilder AddConditionNearbyPlayersCarryItem(this ILocalSpawnBuilder builder, int distance, params string[] itemPrefabNames)
    {
        builder.SetCondition(new ConditionNearbyPlayersCarryItem(distance, itemPrefabNames));
        return builder;
    }

    /// <summary>
    /// Template only enabled when players within <c>distance</c> of spawner has a combined inventory value greater than <c>combinedValueRequired</c>.
    /// </summary>
    public static ILocalSpawnBuilder SetConditionNearbyPlayersCarryValue(this ILocalSpawnBuilder builder, int distance, int combinedValueRequired)
    {
        builder.SetCondition(new ConditionNearbyPlayersCarryValue(distance, combinedValueRequired));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner emits more noise than <c>noiseRequired</c>.
    /// </summary>
    public static ILocalSpawnBuilder SetConditionNearbyPlayersNoise(this ILocalSpawnBuilder builder, int distance, int noiseRequired)
    {
        builder.SetCondition(new ConditionNearbyPlayersNoise(distance, noiseRequired));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has a status in <c>statusEffectNames</c>.
    /// </summary>
    public static ILocalSpawnBuilder AddConditionNearbyPlayersStatus(this ILocalSpawnBuilder builder, int distance, IEnumerable<string> statusEffectNames)
    {
        builder.SetCondition(new ConditionNearbyPlayersStatus(distance, statusEffectNames?.ToArray()));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has a status in <c>statusEffectNames</c>.
    /// </summary>
    public static ILocalSpawnBuilder AddConditionNearbyPlayersStatus(this ILocalSpawnBuilder builder, int distance, params string[] statusEffectNames)
    {
        builder.SetCondition(new ConditionNearbyPlayersStatus(distance, statusEffectNames));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has a status in <c>statusEffects</c>.
    /// </summary>
    public static ILocalSpawnBuilder SetConditionNearbyPlayersStatus(this ILocalSpawnBuilder builder, int distance, IEnumerable<StatusEffect> statusEffects)
    {
        builder.SetCondition(new ConditionNearbyPlayersStatus(distance, statusEffects?.Select(x => x.ToString()).ToArray()));
        return builder;
    }

    /// <summary>
    /// Template only enabled when a player within <c>distance</c> of spawner has a status in <c>statusEffects</c>.
    /// </summary>
    public static ILocalSpawnBuilder SetConditionNearbyPlayersStatus(this ILocalSpawnBuilder builder, int distance, params StatusEffect[] statusEffects)
    {
        builder.SetCondition(new ConditionNearbyPlayersStatus(distance, statusEffects?.Select(x => x.ToString()).ToArray()));
        return builder;
    }

    /// <summary>
    /// Template only enabled when the world day is within the indicated range.
    /// </summary>
    /// <param name="minDays">Minimum days required before template is activated.</param>
    /// <param name="maxDays">Maximum days after which template is deactivated. If null, there is no limit.</param>
    public static ILocalSpawnBuilder SetConditionWorldAge(this ILocalSpawnBuilder builder, int? minDays = null, int? maxDays = null)
    {
        builder.SetCondition(new ConditionWorldAge(minDays, maxDays));
        return builder;
    }
}
