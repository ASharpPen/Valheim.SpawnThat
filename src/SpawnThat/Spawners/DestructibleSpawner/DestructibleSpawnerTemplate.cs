using System;
using System.Collections.Generic;
using SpawnThat.Options.Identifiers;

namespace SpawnThat.Spawners.DestructibleSpawner;

/// <summary>
/// Configurations for spawner. Spawner can have multiple spawn templates.
/// </summary>
internal class DestructibleSpawnerTemplate
{
    public ICollection<ISpawnerIdentifier> Identifiers { get; set; } = new List<ISpawnerIdentifier>(0);

    public string TemplateName { get; set; }

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
    /// <para>Distance within which another entity is counted as being close to spawner.</para>
    /// <para>Default if new template: 1000</para>
    /// </summary>
    /// <remarks>Vanilla name: m_farRadius</remarks>
    public float? DistanceConsideredFar { get; set; }

    /// <summary>
    /// <para>Only spawn spawn if spawn point is not blocked.</para>
    /// <para>Default if new template: true</para>
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
    public bool RemoveNotConfiguredSpawns { get; set; }

    public Dictionary<uint, DestructibleSpawnTemplate> Spawns { get; set; } = new();
}
