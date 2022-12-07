using System;
using System.Collections.Generic;

namespace SpawnThat.Spawners.WorldSpawner;

public static class IWorldSpawnBuilderSettingsExtensions
{
    public static IWorldSpawnBuilder WithSettings(this IWorldSpawnBuilder builder, WorldSpawnSettings settings)
    {
        settings.PrefabName.Set(builder.SetPrefabName);
        settings.Enabled.Set(builder.SetEnabled);
        settings.Biomes.Set(builder.SetConditionBiomes);
        settings.MaxSpawned.SetNullable(builder.SetMaxSpawned);
        settings.SpawnInterval.Set(builder.SetSpawnInterval);
        settings.SpawnChance.Set(builder.SetSpawnChance);
        settings.PackSpawnCircleRadius.Set(builder.SetPackSpawnCircleRadius);
        settings.PackSizeMin.SetNullable(builder.SetPackSizeMin);
        settings.PackSizeMax.SetNullable(builder.SetPackSizeMax);
        settings.SpawnInForest.SetNullable(builder.SetSpawnInForest);
        settings.SpawnOutsideForest.SetNullable(builder.SetSpawnOutsideForest);
        settings.DistanceToCenterForLevelUp.Set(builder.SetDistanceToCenterForLevelUp);
        settings.MinLevel.SetNullable(builder.SetMinLevel);
        settings.MaxLevel.SetNullable(builder.SetMaxLevel);
        settings.LevelUpChance.SetNullable(builder.SetLevelUpChance);
        settings.ConditionMinAltitude.SetNullable(builder.SetConditionAltitudeMin);
        settings.ConditionMaxAltitude.SetNullable(builder.SetConditionAltitudeMax);
        settings.ConditionMinOceanDepth.SetNullable(builder.SetConditionOceanDepthMin);
        settings.ConditionMaxOceanDepth.SetNullable(builder.SetConditionOceanDepthMax);
        settings.ConditionMinTilt.SetNullable(builder.SetConditionTiltMin);
        settings.ConditionMaxTilt.SetNullable(builder.SetConditionTiltMax);
        settings.ConditionEnvironments.Set(builder.SetConditionEnvironments);
        settings.ConditionRequiredGlobalKey.Set(builder.SetConditionRequiredGlobalKey);
        settings.SpawnDuringDay.SetNullable(builder.SetSpawnDuringDay);
        settings.SpawnDuringNight.SetNullable(builder.SetSpawnDuringNight);
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
            builder.SetModifier(modifier);
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

    private static void SetNullable(this bool? value, Func<bool?, IWorldSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this bool? value, Func<bool, IWorldSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this int? value, Func<int?, IWorldSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this float? value, Func<float?, IWorldSpawnBuilder> apply)
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

    private static void SetNullable(this int? value, Func<uint?, IWorldSpawnBuilder> apply)
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

    private static void Set(this TimeSpan? value, Func<TimeSpan?, IWorldSpawnBuilder> apply)
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
