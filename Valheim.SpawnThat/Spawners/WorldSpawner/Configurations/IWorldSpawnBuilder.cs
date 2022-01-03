﻿using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Spawners.Conditions;
using Valheim.SpawnThat.Spawners.Modifiers;
using static Heightmap;

namespace Valheim.SpawnThat.Spawners.WorldSpawner;

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

    IWorldSpawnBuilder AddPostConfiguration(Action<WorldSpawnTemplate> configure);

    IWorldSpawnBuilder SetEnabled(bool enabled);

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

    IWorldSpawnBuilder SetConditionRequiredGlobalKey(string globalKey);

    IWorldSpawnBuilder SetConditionAllowDuringDay(bool allowSpawnDuringDay);

    IWorldSpawnBuilder SetConditionAllowDuringNight(bool allowSpawnDuringNight);

    IWorldSpawnBuilder SetSpawnChance(float spawnChance);

    IWorldSpawnBuilder SetMinDistanceToOther(float distance);

    IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMin(float? distance);

    IWorldSpawnBuilder SetSpawnAtDistanceToPlayerMax(float? distance);

    IWorldSpawnBuilder SetSpawnAtDistanceToGround(float? offset);
}