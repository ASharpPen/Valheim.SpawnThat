using System;

namespace SpawnThat.Spawners.DestructibleSpawner;

/// <summary>
/// Configuration for a destructible spawner (eg. a greydwarf nest).
/// 
/// To identify which spawner to modify, set <c>ISpawnerIdentifier</c>'s 
/// on the builder.
/// When multiple configurations exist for the same spawner, the identifiers
/// and their weights will be used to select the most specific configuration.
/// 
/// Spawner controls the general settings for when things should spawn.
/// It selects what to spawn from a list of spawns, which can be modified
/// or added to by using <c>IDestructibleSpawnBuilder</c>.
/// </summary>
public interface IDestructibleSpawnerBuilder
    : IHaveSpawnerIdentifiers
{
    /// <summary>
    /// Get or create a spawn builder for spawner.
    /// 
    /// If id matches the index of an existing spawn, the existing spawn will be
    /// overridden by the assigned settings of the builder.
    /// </summary>
    IDestructibleSpawnBuilder GetSpawnBuilder(uint id);

    /// <summary>
    /// <para>Chance to level up from MinLevel. Range 0 to 100.</para>
    /// <para>
    ///     Level is assigned by rolling levelup-chance for each
    ///     level from min, until max is reached.
    /// </para>
    /// <para>
    ///     This means if levelup chance is 10, there is 10% chance for
    ///     a MinLevel 1 to become level 2, and 1% chance to become level 3.
    /// </para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <remarks>Vanilla name: m_levelupChance</remarks>
    IDestructibleSpawnerBuilder SetLevelUpChance(float? levelUpChance);

    /// <summary>
    /// Time between spawn checks.
    /// </summary>
    /// <remarks>Vanilla name: m_spawnIntervalSec</remarks>
    IDestructibleSpawnerBuilder SetSpawnInterval(TimeSpan? interval);

    /// <summary>
    /// Sets if spawn should patrol its spawn point.
    /// </summary>
    /// <remarks>Vanilla name: m_setPatrolSpawnPoint</remarks>
    IDestructibleSpawnerBuilder SetPatrol(bool? patrolSpawn);

    /// <summary>
    /// <para>Minimum distance to player for enabling spawn.</para>
    /// </summary>
    /// <remarks>Vanilla name: m_triggerDistance</remarks>
    IDestructibleSpawnerBuilder SetConditionPlayerWithinDistance(float? withinDistance);

    /// <summary>
    /// Sets maximum number of creatures currently loaded,
    /// for spawner to be active.
    /// </summary>
    /// <remarks>Vanilla name: m_maxTotal</remarks>
    IDestructibleSpawnerBuilder SetConditionMaxCloseCreatures(int? maxCloseCreatures);

    /// <summary>
    /// Sets maximum number of creatures currently loaded,
    /// for spawner to be active.
    /// </summary>
    /// <remarks>Vanilla name: m_maxTotal</remarks>
    IDestructibleSpawnerBuilder SetConditionMaxCreatures(int? maxCreatures);

    /// <summary>
    /// <para>Distance within which another entity is counted as being close to spawner.</para>
    /// </summary>
    /// <remarks>Vanilla name: m_nearRadius</remarks>
    IDestructibleSpawnerBuilder SetDistanceConsideredClose(float? distance);

    /// <summary>
    /// <para>Distance within which another entity is counted as being close to spawner.</para>
    /// </summary>
    /// <remarks>Vanilla name: m_farRadius</remarks>
    IDestructibleSpawnerBuilder SetDistanceConsideredFar(float? distance);

    /// <summary>
    /// <para>Only spawn if spawn point is on the ground (ie., not in a dungeon) 
    /// and open to the sky.</para>
    /// <para>For reference, greydwarf nests set this to true, while draugr piles in villages and dungeons do not.</para>
    /// </summary>
    /// <remarks>Vanilla name: m_onGroundOnly</remarks>
    IDestructibleSpawnerBuilder SetOnGroundOnly(bool? checkGround);
}
