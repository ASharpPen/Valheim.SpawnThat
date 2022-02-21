using System.Collections.Generic;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;

namespace SpawnThat.Spawners.DestructibleSpawner;

/// <summary>
/// <para>
///     Destructible spawn builder settings.
///     Any property not set will be ignored.
/// </para>
/// <para>
///     Any setting not set for a builder will mean Spawn That will use the existing setting
///     when overriding a template, or set a default value if creating a new.
/// </para>
/// <para>
///     Intended as an optional way to configure IDestructibleSpawnBuilder.
/// </para>
/// </summary>
public class DestructibleSpawnSettings
{    
    /// <summary>
    /// Id of spawn entry.
    /// 
    /// If id matches the index of an existing entry, the existing entry will be
    /// overridden by the assigned settings of this configuration.
    /// </summary>
    //public uint Id { get; set; }

    /// <summary>
    /// Toggles this template.
    /// If disabled, this spawn entry will never be selected for spawning.
    /// Can be used to disable existing spawn entries.
    /// <para>Default if new template: true</para>
    /// </summary>
    public bool? Enabled { get; set; }

    /// <summary>
    /// <para>
    ///     Toggles this configuration on / off.
    ///     If disabled, template will be ignored.
    ///     Cannot be used to disable existing spawn templates.
    /// </para>
    /// <para>Default if new template: true</para>
    /// </summary>
    public bool? TemplateEnabled { get; set; }

    /// <summary>   
    /// <para>Prefab name of entity to spawn.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// </summary>
    public string PrefabName { get; set; }

    /// <summary>
    /// <para>
    ///     Sets spawn weight. Destructible spawners choose their next
    ///     spawn by a weighted random of all their possible spawns.
    ///     Increasing weight, means an increased chance that this particular
    ///     spawn will be selected for spawning.
    /// </para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_weight</remarks>
    public float? SpawnWeight { get; set; }

    /// <summary>
    /// <para>Minimum level to spawn at.</para>
    /// <para>
    ///     Level is assigned by rolling levelup-chance for each
    ///     level from min, until max is reached.
    /// </para>
    /// <para>
    ///     This means if levelup chance is 10, there is 10% chance for
    ///     a MinLevel 1 to become level 2, and 1% chance to become level 3.
    /// </para>
    /// <para>If not set, uses existing.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minLevel</remarks>
    public int? LevelMin { get; set; }

    /// <summary>
    /// <para>Maximum level to spawn at.</para>
    /// <para>
    ///     Level is assigned by rolling levelup-chance for each
    ///     level from min, until max is reached.
    /// </para>
    /// <para>
    ///     This means if levelup chance is 10, there is 10% chance for
    ///     a MinLevel 1 to become level 2, and 1% chance to become level 3.
    /// </para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_maxLevel</remarks>
    public int? LevelMax { get; set; }

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
}
