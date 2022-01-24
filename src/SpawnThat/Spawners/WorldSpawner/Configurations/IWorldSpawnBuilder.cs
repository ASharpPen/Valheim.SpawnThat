using System;
using System.Collections.Generic;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;
using SpawnThat.Utilities.Enums;
using static Heightmap;

namespace SpawnThat.Spawners.WorldSpawner;

public interface IWorldSpawnBuilder
{
    IWorldSpawnBuilder AddCondition(ISpawnCondition condition);

    IWorldSpawnBuilder AddPositionCondition(ISpawnPositionCondition condition);

    IWorldSpawnBuilder AddModifier(ISpawnModifier modifier);

    IWorldSpawnBuilder AddOrReplaceCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition;

    IWorldSpawnBuilder AddOrReplacePositionCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnPositionCondition;

    IWorldSpawnBuilder AddOrReplaceModifier<TModifier>(TModifier modifier)
        where TModifier : class, ISpawnModifier;

    /*
    /// <summary>
    /// Late running configurations. Run after configurations registered by
    /// <c>SpawnerConfigurationManager.OnConfigure</c> and <c>SpawnerConfigurationManager.SubscribeConfiguration</c>
    /// have been applied.
    /// </summary>
    IWorldSpawnBuilder AddPostConfiguration(Action<WorldSpawnTemplate> configure);
    */

    IWorldSpawnBuilder SetEnabled(bool enabled);

    IWorldSpawnBuilder SetTemplateEnabled(bool enabled);

    IWorldSpawnBuilder SetPrefabName(string prefabName);

    IWorldSpawnBuilder SetTemplateName(string templateName);

    IWorldSpawnBuilder SetConditionBiomes(IEnumerable<Biome> biomes);
    IWorldSpawnBuilder SetConditionBiomes(params Biome[] biomes);
    IWorldSpawnBuilder SetConditionBiomes(IEnumerable<string> biomeNames);
    IWorldSpawnBuilder SetConditionBiomes(params string[] biomeNames);
    IWorldSpawnBuilder SetConditionBiomesAll();

    IWorldSpawnBuilder SetModifierHuntPlayer(bool huntPlayer);

    IWorldSpawnBuilder SetMaxSpawned(uint maxSpawned);

    IWorldSpawnBuilder SetSpawnInterval(TimeSpan interval);

    IWorldSpawnBuilder SetPackSizeMin(uint packSizeMin);

    IWorldSpawnBuilder SetPackSizeMax(uint packSizeMax);

    IWorldSpawnBuilder SetPackSpawnCircleRadius(float radius);

    IWorldSpawnBuilder SetConditionAllowInForest(bool allowSpawnInForest);

    IWorldSpawnBuilder SetConditionAllowOutsideForest(bool allowSpawnOutsideForest);

    IWorldSpawnBuilder SetMinLevel(uint minLevel);

    IWorldSpawnBuilder SetMaxLevel(uint maxLevel);

    IWorldSpawnBuilder SetLevelUpDistance(float distance);

    IWorldSpawnBuilder SetConditionAltitude(float? minAltitude, float? maxAltitude);

    IWorldSpawnBuilder SetConditionOceanDepth(float? minOceanDepth, float? maxOceanDepth);

    IWorldSpawnBuilder SetConditionTilt(float? minTilt, float? maxTilt);

    IWorldSpawnBuilder SetConditionEnvironments(IEnumerable<string> environmentNames);
    IWorldSpawnBuilder SetConditionEnvironments(params string[] environmentNames);
    IWorldSpawnBuilder SetConditionEnvironments(params EnvironmentName[] environmentNames);

    IWorldSpawnBuilder SetConditionRequiredGlobalKey(string globalKey);

    IWorldSpawnBuilder SetConditionAllowDuringDay(bool allowSpawnDuringDay);

    IWorldSpawnBuilder SetConditionAllowDuringNight(bool allowSpawnDuringNight);

    IWorldSpawnBuilder SetSpawnChance(float spawnChance);

    IWorldSpawnBuilder SetMinDistanceToOther(float distance);

    IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMin(float? distance);

    IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMax(float? distance);

    IWorldSpawnBuilder SetSpawnAtDistanceToGround(float? offset);
}
