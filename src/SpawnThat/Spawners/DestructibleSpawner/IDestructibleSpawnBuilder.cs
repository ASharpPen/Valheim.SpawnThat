namespace SpawnThat.Spawners.DestructibleSpawner;

/// <summary>
/// Builder for the individual spawn entries of a destructible spawner.
/// 
/// Multiple entries can exist pr spawner, and will be selected based on weight,
/// after filtering for conditions.
/// </summary>
public interface IDestructibleSpawnBuilder
    : IHaveSpawnConditions
    , IHaveSpawnPositionConditions
    , IHaveSpawnModifiers
{
    /// <summary>
    /// Id of spawn entry.
    /// 
    /// If id matches the index of an existing entry, the existing entry will be
    /// overridden by the assigned settings of this configuration.
    /// </summary>
    uint Id { get; }

    /// <summary>
    /// Toggles this template.
    /// If disabled, this spawn entry will never be selected for spawning.
    /// Can be used to disable existing spawn entries.
    /// <para>Default if new template: true</para>
    /// </summary>
    IDestructibleSpawnBuilder SetEnabled(bool enabled);

    /// <summary>
    /// <para>
    ///     Toggles this configuration on / off.
    ///     If disabled, template will be ignored.
    ///     Cannot be used to disable existing spawn templates.
    /// </para>
    /// <para>Default if new template: true</para>
    /// </summary>
    IDestructibleSpawnBuilder SetTemplateEnabled(bool enabled);

    /// <summary>   
    /// <para>Prefab name of entity to spawn.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// </summary>
    IDestructibleSpawnBuilder SetPrefabName(string prefabName);

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
    IDestructibleSpawnBuilder SetSpawnWeight(float? SpawnWeight);

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
    IDestructibleSpawnBuilder SetLevelMin(int? minLevel);

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
    IDestructibleSpawnBuilder SetLevelMax(int? maxLevel);
}
