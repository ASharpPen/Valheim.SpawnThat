using System;
using System.Collections.Generic;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;
using SpawnThat.Utilities.Enums;
using static Heightmap;

namespace SpawnThat.Spawners.WorldSpawner;

/// <summary>
/// Configuration for a world spawner entry.
/// </summary>
public interface IWorldSpawnBuilder
{
    /// <summary>
    /// <para>Template id.</para>
    /// <para>
    ///     Amongst other things, used to merge configurations, identify if an existing
    ///     entry should be overridden or if a new one should be added.
    /// </para>
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Adds an ISpawnCondition to the builder.
    /// If a condition with the same type already exists, it will be replaced by this one.
    /// </summary>
    IWorldSpawnBuilder SetCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition;

    /// <summary>
    /// Adds an ISpawnPositionCondition to the builder.
    /// If a condition with the same type already exists, it will be replaced by this one.
    /// </summary>
    IWorldSpawnBuilder SetPositionCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnPositionCondition;

    /// <summary>
    /// Adds an ISpawnModifier to the builder.
    /// If a modifier with the same type already exists, it will be replaced by this one.
    /// </summary>
    IWorldSpawnBuilder SetModifier<TModifier>(TModifier modifier)
        where TModifier : class, ISpawnModifier;

    /*
    /// <summary>
    /// Late running configurations. Run after configurations registered by
    /// <c>SpawnerConfigurationManager.OnConfigure</c> and <c>SpawnerConfigurationManager.SubscribeConfiguration</c>
    /// have been applied.
    /// </summary>
    IWorldSpawnBuilder AddPostConfiguration(Action<WorldSpawnTemplate> configure);
    */

    /// <summary>
    /// Toggles this template.
    /// If disabled, this spawn entry will not run.
    /// Can be used to disable existing spawn entries.
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_enabled</remarks>
    IWorldSpawnBuilder SetEnabled(bool enabled);

    /// <summary>
    /// Toggles this configuration on / off.
    /// If disabled, template will be ignored.
    /// Cannot be used to disable existing spawn templates.
    /// <para>Default if new template: ""</para>
    /// </summary>
    IWorldSpawnBuilder SetTemplateEnabled(bool enabled);

    /// <summary>   
    /// <para>Prefab name of entity to spawn.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// </summary>
    IWorldSpawnBuilder SetPrefabName(string prefabName);

    /// <summary>
    /// <para>Just a field for setting a custom name for the template. This is just for debugging.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// </summary>
    IWorldSpawnBuilder SetTemplateName(string templateName);

    /// <summary>
    /// <para>List of biomes in which template is active.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: All biomes enabled</para>
    /// </summary>
    /// <remarks>Vanilla name: m_biome</remarks>
    IWorldSpawnBuilder SetConditionBiomes(IEnumerable<Biome> biomes);
    /// <summary>
    /// <para>List of biomes in which template is active.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: All biomes enabled</para>
    /// </summary>
    /// <remarks>Vanilla name: m_biome</remarks>
    IWorldSpawnBuilder SetConditionBiomes(params Biome[] biomes);
    /// <summary>
    /// <para>List of biomes in which template is active.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: All biomes enabled</para>
    /// </summary>
    /// <remarks>Vanilla name: m_biome</remarks>
    IWorldSpawnBuilder SetConditionBiomes(IEnumerable<string> biomeNames);
    /// <summary>
    /// <para>List of biomes in which template is active.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: All biomes enabled</para>
    /// </summary>
    /// <remarks>Vanilla name: m_biome</remarks>
    IWorldSpawnBuilder SetConditionBiomes(params string[] biomeNames);
    /// <summary>
    /// <para>Sets template as active in all biomes.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: All biomes enabled</para>
    /// </summary>
    /// <remarks>Vanilla name: m_biome</remarks>
    IWorldSpawnBuilder SetConditionBiomesAll();

    /// <summary>
    /// <para>Sets MonsterAI to hunt a player target.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: false</para>
    /// </summary>
    /// <remarks>Vanilla name: m_huntPlayer</remarks>
    IWorldSpawnBuilder SetModifierHuntPlayer(bool huntPlayer);

    IWorldSpawnBuilder SetModifierHuntPlayer(bool? huntPlayer);

    /// <summary>
    /// <para>Maxium number of same entity in area for template to be active.</para>
    /// <para>For creatures, area is defined by currently loaded. For non-creatures, it is defined by distance.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_maxSpawned</remarks>
    IWorldSpawnBuilder SetMaxSpawned(uint maxSpawned);

    IWorldSpawnBuilder SetMaxSpawned(uint? maxSpawned);

    /// <summary>
    /// <para>Time between new spawn checks.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 00:01:30</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnInterval</remarks>
    IWorldSpawnBuilder SetSpawnInterval(TimeSpan interval);

    IWorldSpawnBuilder SetSpawnInterval(TimeSpan? interval);

    /// <summary>
    /// <para>Minimum number of entities to attempt to spawn at a time.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_groupSizeMin</remarks>
    IWorldSpawnBuilder SetPackSizeMin(uint packSizeMin);

    IWorldSpawnBuilder SetPackSizeMin(uint? packSizeMin);

    /// <summary>
    /// <para>Maximum number of entities to attempt to spawn at a time.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_groupSizeMax</remarks>
    IWorldSpawnBuilder SetPackSizeMax(uint packSizeMax);

    IWorldSpawnBuilder SetPackSizeMax(uint? packSizeMax);

    /// <summary>
    /// <para>Radius of circle, in which to spawn a pack of entities.</para>
    /// <para>Eg., when pack size is 3, all 3 spawns will happen inside a circle indicated by this radius.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 3</para>
    /// </summary>
    /// <remarks>Vanilla name: m_groupRadius</remarks>
    IWorldSpawnBuilder SetPackSpawnCircleRadius(float radius);

    IWorldSpawnBuilder SetPackSpawnCircleRadius(float? radius);

    /// <summary>
    /// <para>If true, allows spawning inside forested areas.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_inForest</remarks>
    IWorldSpawnBuilder SetSpawnInForest(bool allowSpawnInForest);

    IWorldSpawnBuilder SetSpawnInForest(bool? allowSpawnInForest);

    /// <summary>
    /// <para>If true, allows pawning outside forested areas.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_outsideForest</remarks>
    IWorldSpawnBuilder SetSpawnOutsideForest(bool allowSpawnOutsideForest);

    IWorldSpawnBuilder SetSpawnOutsideForest(bool? allowSpawnOutsideForest);

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
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minLevel</remarks>
    IWorldSpawnBuilder SetMinLevel(uint minLevel);

    IWorldSpawnBuilder SetMinLevel(uint? minLevel);

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
    IWorldSpawnBuilder SetMaxLevel(uint maxLevel);

    IWorldSpawnBuilder SetMaxLevel(uint? maxLevel);

    /// <summary>
    /// <para>Chance to level up.</para>
    /// <para>Valheim treats 0 or less as 10.</para>
    /// <para>
    ///     Level is assigned by rolling levelup-chance for each
    ///     level from min, until max is reached.
    /// </para>
    /// <para>
    ///     This means if levelup chance is 10 (default), there is 10% chance for
    ///     a MinLevel 1 to become level 2, and 1% chance to become level 3.
    /// </para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: -1</para>
    /// </summary>
    /// <remarks>Vanilla name: m_overrideLevelupChance</remarks>
    IWorldSpawnBuilder SetLevelUpChance(float? chance);

    /// <summary>
    /// <para>Minimum distance for creature to increase level beyond MinLevel.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_levelUpMinCenterDistance</remarks>
    IWorldSpawnBuilder SetDistanceToCenterForLevelUp(float distance);

    IWorldSpawnBuilder SetDistanceToCenterForLevelUp(float? distance);

    /// <summary>
    /// <para>Set altitude (distance to water surface) range enabling spawn.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: -1000, 1000</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minAltitude</remarks>
    /// <remarks>Vanilla name: m_maxAltitude</remarks>
    IWorldSpawnBuilder SetConditionAltitude(float? minAltitude, float? maxAltitude);

    /// <summary>
    /// <para>Minimum altitude (distance to water surface) to spawn at.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: -1000</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minAltitude</remarks>
    IWorldSpawnBuilder SetConditionAltitudeMin(float minAltitude);

    IWorldSpawnBuilder SetConditionAltitudeMin(float? minAltitude);

    /// <summary>
    /// <para>Maximum altitude (distance to water surface) to spawn at.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 1000</para>
    /// </summary>
    /// <remarks>Vanilla name: m_maxAltitude</remarks>
    IWorldSpawnBuilder SetConditionAltitudeMax(float maxAltitude);

    IWorldSpawnBuilder SetConditionAltitudeMax(float? maxAltitude);

    /// <summary>
    /// <para>Ocean depth range to spawn at. Ignored if min == max.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minOceanDepth</remarks>
    /// <remarks>Vanilla name: m_maxOceanDepth</remarks>
    IWorldSpawnBuilder SetConditionOceanDepth(float? minOceanDepth, float? maxOceanDepth);

    /// <summary>
    /// <para>Minimum ocean depth to spawn at. Ignored if min == max.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minOceanDepth</remarks>
    IWorldSpawnBuilder SetConditionOceanDepthMin(float minOceanDepth);

    IWorldSpawnBuilder SetConditionOceanDepthMin(float? minOceanDepth);

    /// <summary>
    /// <para>Maximum ocean depth to spawn at. Ignored if min == max.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_maxOceanDepth</remarks>
    IWorldSpawnBuilder SetConditionOceanDepthMax(float maxOceanDepth);

    IWorldSpawnBuilder SetConditionOceanDepthMax(float? maxOceanDepth);

    /// <summary>
    /// <para>Tilt of terrain required to spawn. Range 0 to 90.</para>
    /// <para>Position condition. Tested multiple times to decide where to spawn entity.</para>
    /// <para>Note: As of this writing, the vanilla calculation is bugged. Be careful with this setting.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: 0, 35</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minTilt</remarks>
    /// <remarks>Vanilla name: m_maxTilt</remarks>
    IWorldSpawnBuilder SetConditionTilt(float? minTilt, float? maxTilt);

    /// <summary>
    /// <para>Minimum tilt of terrain required to spawn. Range 0 to 90.</para>
    /// <para>Position condition. Tested multiple times to decide where to spawn entity.</para>
    /// <para>Note: As of this writing, the vanilla calculation is bugged. Be careful with this setting.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_minTilt</remarks>
    IWorldSpawnBuilder SetConditionTiltMin(float minTilt);

    IWorldSpawnBuilder SetConditionTiltMin(float? minTilt);

    /// <summary>
    /// <para>Maximum tilt of terrain required to spawn. Range 0 to 90.</para>
    /// <para>Position condition. Tested multiple times to decide where to spawn entity.</para>
    /// <para>Note: As of this writing, the vanilla calculation is bugged. Be careful with this setting.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: 35</para>
    /// </summary>
    /// <remarks>Vanilla name: m_maxTilt</remarks>
    IWorldSpawnBuilder SetConditionTiltMax(float maxTilt);

    IWorldSpawnBuilder SetConditionTiltMax(float? maxTilt);

    /// <summary>
    /// <para>List of environments in which spawn is active.</para>
    /// <para>Eg. "Clear", "Misty", "Heath clear".</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: No required environments</para>
    /// </summary>
    /// <remarks>Vanilla name: m_requiredEnvironments</remarks>
    IWorldSpawnBuilder SetConditionEnvironments(IEnumerable<string> environmentNames);

    /// <summary>
    /// <para>List of environments in which spawn is active.</para>
    /// <para>Eg. "Clear", "Misty", "Heath clear".</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: No required environments</para>
    /// </summary>
    /// <remarks>Vanilla name: m_requiredEnvironments</remarks>
    IWorldSpawnBuilder SetConditionEnvironments(params string[] environmentNames);

    /// <summary>
    /// <para>List of environments in which spawn is active.</para>
    /// <para>Eg. "Clear", "Misty", "Heath clear".</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: No required environments</para>
    /// </summary>
    /// <remarks>Vanilla name: m_requiredEnvironments</remarks>
    IWorldSpawnBuilder SetConditionEnvironments(params EnvironmentName[] environmentNames);

    /// <summary>
    /// <para>Global key required for spawn to be active.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: No required global key</para>
    /// </summary>
    /// <remarks>Vanilla name: m_requiredGlobalKey</remarks>
    IWorldSpawnBuilder SetConditionRequiredGlobalKey(string globalKey);

    /// <summary>
    /// <para>Global key required for spawn to be active.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: No required global key</para>
    /// </summary>
    /// <remarks>Vanilla name: m_requiredGlobalKey</remarks>
    IWorldSpawnBuilder SetConditionRequiredGlobalKey(GlobalKey globalKey);

    IWorldSpawnBuilder SetConditionRequiredGlobalKey(GlobalKey? globalKey);

    /// <summary>
    /// <para>Can spawn during day.</para>
    /// <para>Note: If not true, creatures with MonsterAI will attempt to despawn at day.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnAtDay</remarks>
    IWorldSpawnBuilder SetSpawnDuringDay(bool allowSpawnDuringDay);

    IWorldSpawnBuilder SetSpawnDuringDay(bool? allowSpawnDuringDay);

    /// <summary>
    /// <para>Can spawn during night.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: true</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnAtNight</remarks>
    IWorldSpawnBuilder SetSpawnDuringNight(bool allowSpawnDuringNight);

    IWorldSpawnBuilder SetSpawnDuringNight(bool? allowSpawnDuringNight);

    /// <summary>
    /// <para>Chance to spawn per check. Range 0 to 100.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 100</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnChance</remarks>
    IWorldSpawnBuilder SetSpawnChance(float spawnChance);

    IWorldSpawnBuilder SetSpawnChance(float? spawnChance);

    /// <summary>
    /// <para>Minimum required distance to another entity of same type.</para>
    /// <para>Positional condition. Tested multiple times to decide where to spawn entity.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnDistance</remarks>
    IWorldSpawnBuilder SetMinDistanceToOther(float distance);

    IWorldSpawnBuilder SetMinDistanceToOther(float? distance);

    /// <summary>
    /// <para>Minimum distance from player to spawn at.</para>
    /// <para>
    ///     A specific player is chosen as a target, this setting with max basically creates a ring
    ///     around the player, in which a spawn point can be chosen.
    /// </para>
    /// <para>Valheim treats 0 as 40.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnRadiusMin</remarks>
    IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMin(float distance);

    IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMin(float? distance);

    /// <summary>
    /// <para>Maximum distance from player to spawn at.</para>
    /// <para>
    ///     A specific player is chosen as a target, this setting with min basically creates a ring
    ///     around the player, in which a spawn point can be chosen.
    /// </para>
    /// <para>Valheim treats 0 as 80.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: 0</para>
    /// </summary>
    /// <remarks>Vanilla name: m_spawnRadiusMax</remarks>
    IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMax(float distance);

    IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMax(float? distance);


    /// <summary>
    /// <para>Sets how far above the ground the entity will spawn at.</para>
    /// <para>If null, uses existing when overriding.</para>
    /// <para>Default if new template: 0.5</para>
    /// </summary>
    /// <remarks>Vanilla name: m_groundOffset</remarks>
    IWorldSpawnBuilder SetSpawnAtDistanceToGround(float offset);

    IWorldSpawnBuilder SetSpawnAtDistanceToGround(float? offset);

    /// <summary>
    /// <para>Set if allowed to spawn in zone that are edge biomes, in non-edge or both.</para>
    /// <para>If not set, uses existing when overriding.</para>
    /// <para>Default if new template: Heightmap.BiomeArea.Everything</para>
    /// </summary>
    /// <remarks>Vanilla name: m_biomeArea</remarks>
    IWorldSpawnBuilder SetBiomeArea(Heightmap.BiomeArea? biomeArea);
}
