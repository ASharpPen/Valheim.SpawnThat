using System;

namespace SpawnThat.Spawners.LocalSpawner;

public class DefaultLocalSpawnSettings
{
    /// <summary>
    /// Prefab name of entity to spawn.
    /// </summary>
    public string PrefabName { get; set; }

    /// <summary>
    /// <para>Toggle this spawner on-off.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    public bool? Enabled { get; set; }

    /// <summary>
    /// <para>Time between new spawn checks.</para>
    /// <para>Default if new template: 00:20:00</para>
    /// </summary>
    /// <remarks>Vanilla name: m_respawnTimeMinuts</remarks>
    public TimeSpan? SpawnInterval { get; set; }

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
    /// <para>Can spawn during day.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnAtDay</remarks>
    public bool? ConditionAllowDuringDay { get; set; } = true;

    /// <summary>
    /// <para>Can spawn during night.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnAtNight</remarks>
    public bool? ConditionAllowDuringNight { get; set; } = true;

    /// <summary>
    /// <para>Allows spawning if within usual player base protected areas, such as workbench.</para>
    /// <para>Default if new template: false</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnInPlayerBase</remarks>
    public bool? AllowSpawnInPlayerBase { get; set; }

    /// <summary>
    /// <para>Sets patrol point at spawn position. Creatures will run back to this point.</para>
    /// <para>Default if new template: false</para>
    /// </summary>
    /// <remarks>Vanilla name: m_setPatrolSpawnPoint</remarks>
    public bool? SetPatrolSpawn { get; set; }

    /// <summary>
    /// <para>Chance to level up from MinLevel. Range 0 to 100.</para>
    /// <para>
    ///     Level is assigned by rolling levelup-chance for each
    ///     level from min, until max is reached.
    /// </para>
    /// <para>
    ///     This means if levelup chance is 10 (default), there is 10% chance for
    ///     a MinLevel 1 to become level 2, and 1% chance to become level 3.
    /// </para>
    /// <para>Default if new template: 10</para>
    /// </summary>
    public float? LevelUpChance { get; set; }

    /// <summary>
    /// <para>Minimum distance to player for enabling spawn.</para>
    /// <para>Default if new template: 60</para>
    /// </summary>
    /// <remarks>Vanilla name: m_triggerDistance</remarks>
    public float? ConditionPlayerWithinDistance { get; set; }

    /// <summary>
    /// <para>Set spawners "hearing". Only spawn if a player is generating more noise than indicated 
    /// and is within ConditionPlayerDistance of the same distance.
    /// Noise also acts as a distance requirement.</para>
    /// <para>Eg., if 10, a player generating 5 noise will not trigger spawning no matter how close.</para>
    /// <para>Eg., if 10, a player generating 15 noise, must be within 15 distance to spawner.</para>
    /// <see cref="https://github.com/ASharpPen/SpawnThat/wiki/field-options#noise"/>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_triggerNoise</remarks>
    public float? ConditionPlayerNoise { get; set; }
}
