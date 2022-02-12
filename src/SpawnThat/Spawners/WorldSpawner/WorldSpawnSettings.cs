using System;
using System.Collections.Generic;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;

namespace SpawnThat.Spawners.WorldSpawner;

/// <summary>
/// <para>
///     World spawn builder settings.
///     Any property not set will be ignored.
/// </para>
/// <para>
///     Any setting not set for a builder will mean Spawn That will use the existing setting
///     when overriding a template, or set a default value if creating a new.
/// </para>
/// <para>
///     Intended as an optional way to configure IWorldSpawnBuilder.
/// </para>
/// </summary>
public class WorldSpawnSettings
{
    /// <summary>
    /// Spawn conditions to set.
    /// If a condition with the same type already exists, it will be replaced.
    /// </summary>
    public ICollection<ISpawnCondition> Conditions { get; set; } = new List<ISpawnCondition>();

    /// <summary>
    /// Spawn position conditions to set.
    /// If a condition with the same type already exists, it will be replaced.
    /// </summary>
    public ICollection<ISpawnPositionCondition> PositionConditions { get; set; } = new List<ISpawnPositionCondition>();

    /// <summary>
    /// Modifiers conditions to set.
    /// If a condition with the same type already exists, it will be replaced.
    /// </summary>
    public ICollection<ISpawnModifier> Modifiers { get; set; } = new List<ISpawnModifier>();

    /// <summary>
    /// Prefab name of entity to spawn.
    /// </summary>
    public string PrefabName { get; set; }

    /// <summary>
    /// <para>Toggle this spawner on-off.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_enabled</remarks>
    public bool? Enabled { get; set; }

    /// <summary>
    /// <para>List of biomes in which template is active.</para>
    /// <para>Empty list or null will be treated as if property was not set.</para>
    /// <para>Default if new template: All biomes enabled</para>
    /// </summary>
    /// <remarks>Vanilla name: m_biome</remarks>
    public List<Heightmap.Biome> Biomes { get; set; } = new();

    /// <summary>
    /// <para>Sets MonsterAI to hunt a player target.</para>
    /// <para>Default if new template: false</para>
    /// </summary>
    /// <remarks>Vanilla name: m_huntPlayer</remarks>
    public bool? HuntPlayer { get; set; }

    /// <summary>
    /// <para>Maxium number of same entity in area for template to be active.</para>
    /// <para>For creatures, area is defined by currently loaded. For non-creatures, it is defined by distance.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_maxSpawned</remarks>
    public int? MaxSpawned { get; set; }

    /// <summary>
    /// <para>Time between new spawn checks.</para>
    /// <para>Default if new template: 00:01:30</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnInterval</remarks>
    public TimeSpan? SpawnInterval { get; set; }

    /// <summary>
    /// <para>Chance to spawn per check. Range 0 to 100.</para>
    /// <para>Default if new template: 100</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnChance</remarks>
    public float? SpawnChance { get; set; }

    /// <summary>
    /// <para>Radius of circle, in which to spawn a pack of entities.</para>
    /// <para>Eg., when pack size is 3, all 3 spawns will happen inside a circle indicated by this radius.</para>
    /// <para>Default if new template: 3</para>
    /// </summary>
    /// <remarks>Vanilla name: m_groupRadius</remarks>
    public float? PackSpawnCircleRadius { get; set; }

    /// <summary>
    /// <para>Minimum number of entities to attempt to spawn at a time.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_groupSizeMin</remarks>
    public int? PackSizeMin { get; set; }

    /// <summary>
    /// <para>Maximum number of entities to attempt to spawn at a time.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_groupSizeMax</remarks>
    public int? PackSizeMax { get; set; }

    /// <summary>
    /// <para>If true, allows spawning inside forested areas.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_inForest</remarks>
    public bool? SpawnInForest { get; set; }

    /// <summary>
    /// <para>If true, allows pawning outside forested areas.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_outsideForest</remarks>
    public bool? SpawnOutsideForest { get; set; }

    /// <summary>
    /// <para>Minimum distance for creature to increase level beyond MinLevel.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_levelUpMinCenterDistance</remarks>
    public float? DistanceToCenterForLevelUp { get; set; }

    /// <summary>
    /// <para>Minimum level to spawn at.</para>
    /// <para>
    ///     Level is assigned by rolling levelup-chance for each
    ///     level from min, until max is reached.
    /// </para>
    /// <para>
    ///     This means if levelup chance is 10 (default), there is 10% chance for
    ///     a MinLevel 1 to become level 2, and 1% chance to become level 3.
    /// </para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minLevel</remarks>
    public int? MinLevel { get; set; }

    /// <summary>
    /// <para>Maximum level to spawn at.</para>
    /// <para>
    ///     Level is assigned by rolling levelup-chance for each
    ///     level from min, until max is reached.
    /// </para>
    /// <para>
    ///     This means if levelup chance is 10 (default), there is 10% chance for
    ///     a MinLevel 1 to become level 2, and 1% chance to become level 3.
    /// </para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_maxLevel</remarks>
    public int? MaxLevel { get; set; }

    /// <summary>
    /// <para>Minimum altitude (distance to water surface) to spawn at.</para>
    /// <para>Default if new template: -1000</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minAltitude</remarks>
    public float? ConditionMinAltitude { get; set; }

    /// <summary>
    /// <para>Maximum altitude (distance to water surface) to spawn at.</para>
    /// <para>Default if new template: 1000</para>
    /// </summary>
    /// <remarks>Vanilla name: m_maxAltitude</remarks>
    public float? ConditionMaxAltitude { get; set; }

    /// <summary>
    /// <para>Minimum ocean depth to spawn at. Ignored if min == max.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minOceanDepth</remarks>
    public float? ConditionMinOceanDepth { get; set; }

    /// <summary>
    /// <para>Maximum ocean depth to spawn at. Ignored if min == max.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_maxOceanDepth</remarks>
    public float? ConditionMaxOceanDepth { get; set; }

    /// <summary>
    /// <para>Minimum tilt of terrain required to spawn. Range 0 to 90.</para>
    /// <para>Position condition. Tested multiple times to decide where to spawn entity.</para>
    /// <para>Note: As of this writing, the vanilla calculation is bugged. Be careful with this setting.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minTilt</remarks>
    public float? ConditionMinTilt { get; set; }

    /// <summary>
    /// <para>Maximum tilt of terrain required to spawn. Range 0 to 90.</para>
    /// <para>Position condition. Tested multiple times to decide where to spawn entity.</para>
    /// <para>Note: As of this writing, the vanilla calculation is bugged. Be careful with this setting.</para>
    /// <para>Default if new template: 35</para>
    /// </summary>
    /// <remarks>Vanilla name: m_maxTilt</remarks>
    public float? ConditionMaxTilt { get; set; }

    /// <summary>
    /// <para>List of environments in which spawn is active.</para>
    /// <para>Eg. "Clear", "Misty", "Heath clear".</para>
    /// <para>Empty list or null will be treated as if property was not set.</para>
    /// <para>Default if new template: No required environments</para>
    /// </summary>
    /// <remarks>Vanilla name: m_requiredEnvironments</remarks>
    public List<string> ConditionEnvironments { get; set; } = new();

    /// <summary>
    /// <para>Global key required for spawn to be active.</para>
    /// <para>Default if new template: No required global key</para>
    /// </summary>
    /// <remarks>Vanilla name: m_requiredGlobalKey</remarks>
    public string ConditionRequiredGlobalKey { get; set; }

    /// <summary>
    /// <para>Can spawn during day.</para>
    /// <para>Note: If not true, creatures with MonsterAI will attempt to despawn at day.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnAtDay</remarks>
    public bool? SpawnDuringDay { get; set; }

    /// <summary>
    /// <para>Can spawn during night.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnAtNight</remarks>
    public bool? SpawnDuringNight { get; set; }

    /// <summary>
    /// <para>Minimum required distance to another entity of same type.</para>
    /// <para>Positional condition. Tested multiple times to decide where to spawn entity.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnDistance</remarks>
    public float? MinDistanceToOther { get; set; }

    /// <summary>
    /// <para>Minimum distance from player to spawn at.</para>
    /// <para>
    ///     A specific player is chosen as a target, this setting with max basically creates a ring
    ///     around the player, in which a spawn point can be chosen.
    /// </para>
    /// <para>Valheim treats 0 as 40.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnRadiusMin</remarks>
    public float? SpawnAtDistanceToPlayerMin { get; set; }

    /// <summary>
    /// <para>Maximum distance from player to spawn at.</para>
    /// <para>
    ///     A specific player is chosen as a target, this setting with min basically creates a ring
    ///     around the player, in which a spawn point can be chosen.
    /// </para>
    /// <para>Valheim treats 0 as 80.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnRadiusMax</remarks>
    public float? SpawnAtDistanceToPlayerMax { get; set; }

    /// <summary>
    /// <para>Sets how far above the ground the entity will spawn at.</para>
    /// <para>Default if new template: 0.5</para>
    /// </summary>
    /// <remarks>Vanilla name: m_groundOffset</remarks>
    public float? SpawnAtDistanceToGround { get; set; }
}
