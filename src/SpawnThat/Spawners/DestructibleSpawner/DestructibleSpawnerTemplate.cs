using System;
using System.Collections.Generic;
using SpawnThat.Spawners.DestructibleSpawner.Identifiers;

namespace SpawnThat.Spawners.DestructibleSpawner;

/// <summary>
/// Configurations for spawner. Spawner can have multiple spawn templates.
/// </summary>
internal class DestructibleSpawnerTemplate
{
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
    /// Distance within which another entity is counted as being close to spawner.
    /// </summary>
    /// <remarks>Vanilla name: m_nearRadius</remarks>
    public float? DistanceIsClose { get; set; }

    /// <summary>
    /// Only spawn spawn if spawn point is not blocked.
    /// </summary>
    /// <remarks>Vanilla name: m_onGroundOnly</remarks>
    public bool? PositionConditionNotBlocked { get; set; }

    public ICollection<DestructibleSpawnTemplate> Spawns { get; set; } = new List<DestructibleSpawnTemplate>();
}
