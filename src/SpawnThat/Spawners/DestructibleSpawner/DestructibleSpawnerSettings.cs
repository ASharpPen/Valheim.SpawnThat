using System;
using System.Collections.Generic;
using SpawnThat.Options.Identifiers;

namespace SpawnThat.Spawners.SpawnAreaSpawner;

/// <summary>
/// <para>
///     SpawnArea spawner builder settings.
///     Any property not set will be ignored.
/// </para>
/// <para>
///     Any setting not set for a builder will mean Spawn That will use the existing setting
///     when overriding a template, or set a default value if creating a new.
/// </para>
/// <para>
///     Intended as an optional way to configure ISpawnAreaSpawnerBuilder.
/// </para>
/// </summary>
public class SpawnAreaSpawnerSettings
{
    /// <summary>
    /// <para>
    ///     Identifiers used to select which configuration to apply to spawner.
    /// </para>
    /// <para>
    ///     To identify which spawner to modify, set <c>ISpawnerIdentifier</c>'s 
    ///     on the builder.
    ///     When multiple configurations exist for the same spawner, the identifiers
    ///     and their weights will be used to select the most specific configuration.
    /// </para>
    /// </summary>
    public ICollection<ISpawnerIdentifier> Identifiers { get; set; } = new List<ISpawnerIdentifier>(0);

    /// <summary>
    /// Chance for a spawned entity to level up.
    /// </summary>
    /// <remarks>Vanilla name: m_levelupChance</remarks>
    public float? LevelUpChance { get; set; }

    /// <summary>
    /// Time between spawn checks.
    /// </summary>
    /// <remarks>Vanilla name: m_spawnIntervalSec</remarks>
    public TimeSpan? SpawnInterval { get; set; }

    /// <summary>
    /// Sets if spawn should patrol its spawn point.
    /// </summary>
    /// <remarks>Vanilla name: m_setPatrolSpawnPoint</remarks>
    public bool? SetPatrol { get; set; }

    /// <summary>
    /// <para>Minimum distance to player for enabling spawn.</para>
    /// <para>Default if new template: 60</para>
    /// </summary>
    /// <remarks>Vanilla name: m_triggerDistance</remarks>
    public float? ConditionPlayerWithinDistance { get; set; }

    /// <summary>
    /// Sets maximum number of creatures within <c>DistanceIsClose</c>,
    /// for spawner to be active.
    /// </summary>
    /// <remarks>Vanilla name: m_maxNear</remarks>
    public int? ConditionMaxCloseCreatures { get; set; }

    /// <summary>
    /// Sets maximum number of creatures currently loaded,
    /// for spawner to be active.
    /// </summary>
    /// <remarks>Vanilla name: m_maxTotal</remarks>
    public int? ConditionMaxCreatures { get; set; }

    /// <summary>
    /// <para>Distance within which another entity is counted as being close to spawner.</para>
    /// <para>Default if new template: 20</para>
    /// </summary>
    /// <remarks>Vanilla name: m_nearRadius</remarks>
    public float? DistanceConsideredClose { get; set; }

    /// <summary>
    /// <para>Distance within which another entity is counted as being far to spawner.</para>
    /// <para>Default if new template: 1000</para>
    /// </summary>
    /// <remarks>Vanilla name: m_farRadius</remarks>
    public float? DistanceConsideredFar { get; set; }

    /// <summary>
    /// <para>
    ///     Only spawn if spawn point is on the ground (ie., not in a dungeon) 
    ///     and open to the sky.
    /// </para>
    /// <para>
    ///     For reference, greydwarf nests set this to true, while draugr piles in villages and dungeons do not.
    /// </para>
    /// </summary>
    /// <remarks>Vanilla name: m_onGroundOnly</remarks>
    public bool? OnGroundOnly { get; set; }

    /// <summary>
    /// <para>
    ///     Sets if spawns of spawner that were not configured should be removed.
    /// </para>
    /// <para>
    ///     Intended to simplify disabling of undesired spawns. 
    ///     Instead of disabling them index by index, this setting can be used.
    /// </para>
    /// </summary>
    public bool? RemoveNotConfiguredSpawns { get; set; }

    /// <summary>
    /// Spawns for spawner.
    /// 
    /// If id matches the index of an existing spawn, the existing spawn will be
    /// overridden by the assigned settings.
    /// </summary>
    public Dictionary<uint, SpawnAreaSpawnSettings> Spawns { get; set; } = new();
}
