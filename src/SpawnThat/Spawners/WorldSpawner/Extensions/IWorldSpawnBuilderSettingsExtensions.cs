﻿using System;
using System.Collections.Generic;
using SpawnThat.Spawners.WorldSpawner;
using SpawnThat.Spawners.WorldSpawner.Configurations;

namespace SpawnThat.Spawners;

public static class IWorldSpawnBuilderSettingsExtensions
{
    public static IWorldSpawnBuilder WithSettings(this IWorldSpawnBuilder builder, WorldSpawnSettings settings)
    {
        settings.PrefabName.Set(builder.SetPrefabName);
        settings.Enabled.Set(builder.SetEnabled);
        settings.Biomes.Set(builder.SetConditionBiomes);
        settings.MaxSpawned.Set(builder.SetMaxSpawned);
        settings.SpawnInterval.Set(builder.SetSpawnInterval);
        settings.SpawnChance.Set(builder.SetSpawnChance);
        settings.PackSpawnCircleRadius.Set(builder.SetPackSpawnCircleRadius);
        settings.PackSizeMin.Set(builder.SetPackSizeMin);
        settings.PackSizeMax.Set(builder.SetPackSizeMax);
        settings.SpawnInForest.Set(builder.SetSpawnInForest);
        settings.SpawnOutsideForest.Set(builder.SetSpawnOutsideForest);
        settings.DistanceToCenterForLevelUp.Set(builder.SetDistanceToCenterForLevelUp);
        settings.MinLevel.Set(builder.SetMinLevel);
        settings.MaxLevel.Set(builder.SetMaxLevel);
        settings.ConditionMinAltitude.SetNullable(builder.SetConditionAltitudeMin);
        settings.ConditionMaxAltitude.SetNullable(builder.SetConditionAltitudeMax);
        settings.ConditionMinOceanDepth.SetNullable(builder.SetConditionOceanDepthMin);
        settings.ConditionMaxOceanDepth.SetNullable(builder.SetConditionOceanDepthMax);
        settings.ConditionMinTilt.SetNullable(builder.SetConditionTiltMin);
        settings.ConditionMaxTilt.SetNullable(builder.SetConditionTiltMax);
        settings.ConditionEnvironments.Set(builder.SetConditionEnvironments);
        settings.ConditionRequiredGlobalKey.Set(builder.SetConditionRequiredGlobalKey);
        settings.SpawnDuringDay.Set(builder.SetSpawnDuringDay);
        settings.SpawnDuringNight.Set(builder.SetSpawnDuringNight);
        settings.MinDistanceToOther.Set(builder.SetMinDistanceToOther);
        settings.SpawnAtDistanceToPlayerMin.SetNullable(builder.SetSpawnAtDistanceToPlayerMin);
        settings.SpawnAtDistanceToPlayerMax.SetNullable(builder.SetSpawnAtDistanceToPlayerMax);
        settings.SpawnAtDistanceToGround.SetNullable(builder.SetSpawnAtDistanceToGround);

        foreach (var condition in settings.Conditions)
        {
            builder.SetCondition(condition);
        }

        foreach (var condition in settings.PositionConditions)
        {
            builder.SetPositionCondition(condition);
        }

        foreach (var modifier in settings.Modifiers)
        {
            builder = builder.SetModifier(modifier);
        }

        return builder;

    }

    private static void Set(this string value, Func<string, IWorldSpawnBuilder> apply)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            apply(value);
        }
    }

    private static void Set(this bool? value, Func<bool, IWorldSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this int? value, Func<int, IWorldSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this float? value, Func<float, IWorldSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void SetNullable(this float? value, Func<float?, IWorldSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this int? value, Func<uint, IWorldSpawnBuilder> apply)
    {
        if (value is not null)
        {
            if (value >= 0)
            {
                apply((uint)value.Value);
            }
            else
            {
                apply(0);
            }
        }
    }

    private static void Set(this TimeSpan? value, Func<TimeSpan, IWorldSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set<T>(this List<T> value, Func<List<T>, IWorldSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Count > 0)
        {
            apply(value);
        }
    }
}