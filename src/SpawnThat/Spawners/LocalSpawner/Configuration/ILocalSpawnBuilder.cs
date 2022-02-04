using System;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;

namespace SpawnThat.Spawners.LocalSpawner.Configuration;

public interface ILocalSpawnBuilder
{
    /// <summary>
    /// Adds an ISpawnCondition to the builder.
    /// If a condition with the same type already exists, it will be replaced by this one.
    /// </summary>
    ILocalSpawnBuilder SetCondition<TCondition>(TCondition condition)
    where TCondition : class, ISpawnCondition;

    /// <summary>
    /// Adds an ISpawnModifier to the builder.
    /// If a modifier with the same type already exists, it will be replaced by this one.
    /// </summary>
    ILocalSpawnBuilder SetModifier<TModifier>(TModifier modifier)
        where TModifier : class, ISpawnModifier;

    /// <summary>
    /// Toggles this template.
    /// If disabled, will disable the spawner.
    /// <para>Default if new template: true</para>
    /// </summary>
    ILocalSpawnBuilder SetEnabled(bool enabled = true);

    /// <summary>
    /// Toggles this configuration.
    /// If disabled, this template will not be applied.
    /// <para>Default if new template: true</para>
    /// </summary>
    ILocalSpawnBuilder SetTemplateEnabled(bool enabled = true);

    /// <summary>
    /// Prefab name of entity to spawn.
    /// </summary>
    ILocalSpawnBuilder SetPrefabName(string prefabName);

    /// <summary>
    /// <para>Time between new spawn checks.</para>
    /// <para>If null, uses existing.</para>
    /// <para>Default if new template: 00:20:00</para>
    /// </summary>
    /// <remarks>Vanilla name: m_respawnTimeMinuts</remarks>
    ILocalSpawnBuilder SetSpawnInterval(TimeSpan? frequency = null);

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
    /// <para>If not set, uses existing.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minLevel</remarks>
    ILocalSpawnBuilder SetMinLevel(int minLevel = 1);

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
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_maxLevel</remarks>
    ILocalSpawnBuilder SetMaxLevel(int maxLevel = 1);

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
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 10</para>
    /// </summary>
    ILocalSpawnBuilder SetLevelUpChance(float chance);

    /// <summary>
    /// <para>Can spawn during day.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnAtDay</remarks>
    ILocalSpawnBuilder SetSpawnDuringDay(bool allowSpawnDuringDay = true);

    /// <summary>
    /// <para>Can spawn during night.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnAtNight</remarks>
    ILocalSpawnBuilder SetSpawnDuringNight(bool allowSpawnDuringNight = true);

    /// <para>Allows spawning if within usual player base protected areas, such as workbench.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: false</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnInPlayerBase</remarks>
    ILocalSpawnBuilder SetSpawnInPlayerBase(bool spawnInPlayerBase = false);

    /// <summary>
    /// <para>Sets patrol point at spawn position. Creatures will run back to this point.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: false</para>
    /// </summary>
    /// <remarks>Vanilla name: m_setPatrolSpawnPoint</remarks>
    ILocalSpawnBuilder SetPatrolSpawn(bool patrolSpawn = false);

    /// <summary>
    /// <para>Minimum distance to player for enabling spawn.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 60</para>
    /// </summary>
    /// <remarks>Vanilla name: m_triggerDistance</remarks>
    ILocalSpawnBuilder SetConditionPlayerWithinDistance(float distance);

    /// <summary>
    /// <para>Set spawners "hearing". Only spawn if a player is generating more noise than indicated 
    /// and is within ConditionPlayerDistance of the same distance.
    /// Noise also acts as a distance requirement.</para>
    /// <para>Eg., if 10, a player generating 5 noise will not trigger spawning no matter how close.</para>
    /// <para>Eg., if 10, a player generating 15 noise, must be within 15 distance to spawner.</para>
    /// <see cref="https://github.com/ASharpPen/SpawnThat/wiki/field-options#noise"/>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_triggerNoise</remarks>
    ILocalSpawnBuilder SetConditionPlayerNoise(float noise);

    //ILocalSpawnBuilder AddPostConfiguration(Action<LocalSpawnTemplate> configure);
}
