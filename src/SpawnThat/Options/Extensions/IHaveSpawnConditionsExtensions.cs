using System.Collections.Generic;
using System.Linq;
using SpawnThat.Options.Conditions;
using SpawnThat.Spawners.DestructibleSpawner;
using SpawnThat.Utilities.Enums;
using static Heightmap;

namespace SpawnThat.Spawners;

public static class IHaveSpawnConditionsExtensions
{
    /// <summary>
    /// <para> Set allowed altitude range in which spawner is active. </para>
    /// <para> 
    ///     Altitude is calculated as the spawners vertical distance to water-level.
    ///     Meaning altitude 0 is at the water surface. 
    ///     If negative, it means below water, positive means above.
    /// </para>
    /// <para> 
    ///     Note: Depending on spawner type/settings, spawning can still be done at other altitudes. 
    ///     Use <see cref="PositionConditionAltitude"/> to control the allowed spawn positions.
    /// </para>
    /// </summary>
    /// <param name="min">Sets minimum altitude for spawner to be active. Ignored if null.</param>
    /// <param name="max">Sets maximum altitude for spawner to be active. Ignored if null.</param>
    public static T SetConditionAltitude<T>(this T builder, float? min = null, float? max = null)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionAltitude(min, max));
        return builder;
    }

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
    public static IDestructibleSpawnBuilder SetConditionAreaSpawnChance(this IDestructibleSpawnBuilder builder, float areaChance)
    {
        builder.SetCondition(new ConditionAreaSpawnChance(areaChance, (int)builder.Id));
        return builder;
    }

    /// <summary>
    /// <para>Set biomes in which spawning is enabled.</para>
    /// </summary>
    public static T SetConditionBiome<T>(this T builder, params Heightmap.Biome[] biomes)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionBiome(biomes));
        return builder;
    }

    /// <summary>
    /// <para>Set biomes in which spawning is enabled.</para>
    /// </summary>
    public static T SetConditionBiome<T>(this T builder, IEnumerable<Heightmap.Biome> biomes)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionBiome(biomes.ToArray()));
        return builder;
    }

    /// <summary>
    /// <para>Set biomes in which spawning is enabled.</para>
    /// </summary>
    public static T SetConditionBiome<T>(this T builder, params string[] biomeNames)
        where T : IHaveSpawnConditions
{
        builder.SetCondition(new ConditionBiome(biomeNames));
        return builder;
    }

    /// <summary>
    /// <para>Set biomes in which spawning is enabled.</para>
    /// </summary>
    public static T SetConditionBiome<T>(this T builder, IEnumerable<string> biomeNames)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionBiome(biomeNames));
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
    /// <para>Set time periods in which spawning is active.</para>
    /// </summary>
    public static T SetConditionDaytime<T>(this T builder, params Daytime[] daytimes)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionDaytime(daytimes));
        return builder;
    }

    /// <summary>
    /// <para>Set time periods in which spawning is active.</para>
    /// </summary>
    public static T SetConditionDaytime<T>(this T builder, IEnumerable<Daytime> daytimes)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionDaytime(daytimes));
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
    /// <para>Set names of environments in which spawning is active.</para>
    /// <para>Eg. "Clear", "Misty", "Heath clear".</para>
    /// </summary>
    public static T SetConditionEnvironment<T>(this T builder, params string[] environmentNames)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionEnvironment(environmentNames));
        return builder;
    }

    /// <summary>
    /// <para>Set names of environments in which spawning is active.</para>
    /// <para>Eg. "Clear", "Misty", "Heath clear".</para>
    /// </summary>
    public static T SetConditionEnvironment<T>(this T builder, IEnumerable<string> environmentNames)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionEnvironment(environmentNames));
        return builder;
    }

    /// <summary>
    /// <para>Set names of known vanilla environments in which spawning is active.</para>
    /// </summary>
    public static T SetConditionEnvironment<T>(this T builder, params EnvironmentName[] environmentNames)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionEnvironment(environmentNames.Select(x => x.GetName())));
        return builder;
    }

    /// <summary>
    /// <para>Set names of known vanilla environments in which spawning is active.</para>
    /// </summary>
    public static T SetConditionEnvironment<T>(this T builder, IEnumerable<EnvironmentName> environmentNames)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionEnvironment(environmentNames.Select(x => x.GetName())));
        return builder;
    }

    /// <summary>
    /// <para>Set forestation state for which spawning is active.</para>
    /// <para>Note: This is based on worldgeneration, not on actual amount of vegetation present.</para>
    /// </summary>
    public static T SetConditionForest<T>(this T builder, ForestState requiredState)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionForest(requiredState));
        return builder;
    }

    /// <summary>
    /// <para>Set list of global keys, where spawning is active if any are present.</para>
    /// </summary>
    public static T SetConditionAnyOfGlobalKeys<T>(this T builder, params string[] globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysAny(globalKeys));
        return builder;
    }

    /// <summary>
    /// <para>Set list of global keys, where spawning is active if any are present.</para>
    /// </summary>
    public static T SetConditionAnyOfGlobalKeys<T>(this T builder, IEnumerable<string> globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysAny(globalKeys));
        return builder;
    }

    /// <summary>
    /// <para>Set list of known vanilla global keys, where spawning is active if any are present.</para>
    /// </summary>
    public static T SetConditionAnyOfGlobalKeys<T>(this T builder, params GlobalKey[] globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysAny(globalKeys.Select(x => x.GetName())));
        return builder;
    }

    /// <summary>
    /// <para>Set list of known vanilla global keys, where spawning is active if any are present.</para>
    /// </summary>
    public static T SetConditionAnyOfGlobalKeys<T>(this T builder, IEnumerable<GlobalKey> globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysAny(globalKeys.Select(x => x.GetName())));
        return builder;
    }

    /// <summary>
    /// <para>Sets global keys which must all be present for spawning to be active.</para>
    /// </summary>
    public static T SetConditionAllOfGlobalKeys<T>(this T builder, params string[] globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysRequired(globalKeys));
        return builder;
    }

    /// <summary>
    /// <para>Sets global keys which must all be present for spawning to be active.</para>
    /// </summary>
    public static T SetConditionAllOfGlobalKeys<T>(this T builder, IEnumerable<string> globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysRequired(globalKeys));
        return builder;
    }

    /// <summary>
    /// <para>Sets known vanilla global keys which must all be present for spawning to be active.</para>
    /// </summary>
    public static T SetConditionAllOfGlobalKeys<T>(this T builder, params GlobalKey[] globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysRequired(globalKeys.Select(x => x.GetName())));
        return builder;
    }

    /// <summary>
    /// <para>Sets known vanilla global keys which must all be present for spawning to be active.</para>
    /// </summary>
    public static T SetConditionAllOfGlobalKeys<T>(this T builder, IEnumerable<GlobalKey> globalKeys)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionGlobalKeysRequired(globalKeys.Select(x => x.GetName())));
        return builder;
    }

    /// <summary>
    /// <para>Sets global keys which must not be present for spawning to be allowed.</para>
    /// <para>
    ///     Eg., if "KilledTroll", "defeated_eikthyr" and world has global key "defeated_eikthyr",
    ///     then this template will be disabled.
    /// </para>
    /// </summary>
    public static T SetConditionNoneOfGlobalkeys<T>(this T builder, IEnumerable<string> globalKeys)
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
    public static T SetConditionNoneOfGlobalkeys<T>(this T builder, params string[] globalKeys)
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
    public static T SetConditionNoneOfGlobalkeys<T>(this T builder, IEnumerable<GlobalKey> globalKeys)
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
    public static T SetConditionNoneOfGlobalkeys<T>(this T builder, params GlobalKey[] globalKeys)
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
    /// <para>Set allowed ocean depth range in which spawning is active.</para>
    /// <para>Ocean depth is defined as distance to water surface. And is calculated as:</para>
    /// <para><c>Max(0, WaterLevel - Seafloor)</c></para>
    /// <para>
    ///     This means the water-surface and above will be ocean depth 0. 
    ///     An ocean floor at 10 units below the surface will be 10.
    /// </para>
    /// </summary>
    /// <param name="min">Minimum depth required. Ignored if null.</param>
    /// <param name="max">Maximum depth allowed. Ignored if null.</param>
    public static T SetConditionOceanDepth<T>(this T builder, float? min = null, float? max = null)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionOceanDepth(min, max));
        return builder;
    }

    /// <summary>
    /// <para>Set tilt range of terrain, for which spawning is active.</para>
    /// <para>Tilt is calculated as 0 when perfectly flat, and 90 when completely vertical.</para>
    /// <para>
    ///     Note: Valheim calculates tilt extremely inconsistently throughout the codebase.
    ///     Tilt above 45 needs to be tested before being relied upon, as there is at least one
    ///     buggy calculation in vanilla.
    /// </para>
    /// </summary>
    /// <param name="min">Minimum degrees of tilt required. Ignored if null.</param>
    /// <param name="max">Maximum degrees of tilt allowed. Ignored if null.</param>
    public static T SetConditionTilt<T>(this T builder, float? min = null, float? max = null)
        where T : IHaveSpawnConditions
    {
        builder.SetCondition(new ConditionTilt(min, max));
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
