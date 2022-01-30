using System;
using SpawnThat.Spawners.LocalSpawner;
using SpawnThat.Spawners.LocalSpawner.Configuration;

namespace SpawnThat.Spawners;

public static class ILocalSpawnBuilderDefaultSettingsExtensions
{
    public static ILocalSpawnBuilder WithSettings(this ILocalSpawnBuilder builder, DefaultLocalSpawnSettings settings)
    {
        settings.PrefabName.Set(builder.SetPrefabName);
        settings.Enabled.Set(builder.SetEnabled);
        settings.SpawnInterval.SetNullable(builder.SetSpawnInterval);
        settings.MinLevel.Set(builder.SetMinLevel);
        settings.MaxLevel.Set(builder.SetMaxLevel);
        settings.ConditionAllowDuringNight.Set(builder.SetConditionAllowDuringDay);
        settings.ConditionAllowDuringNight.Set(builder.SetConditionAllowDuringNight);
        settings.AllowSpawnInPlayerBase.Set(builder.SetAllowSpawnInPlayerBase);
        settings.SetPatrolSpawn.Set(builder.SetPatrolSpawn);
        settings.LevelUpChance.Set(builder.SetLevelUpChance);
        settings.ConditionPlayerWithinDistance.Set(builder.SetConditionPlayerWithinDistance);
        settings.ConditionPlayerNoise.Set(builder.SetConditionPlayerNoise);

        return builder;
    }

    private static void Set(this string value, Func<string, ILocalSpawnBuilder> apply)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            apply(value);
        }
    }

    private static void Set(this bool? value, Func<bool, ILocalSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this int? value, Func<int, ILocalSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this float? value, Func<float, ILocalSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void SetNullable(this TimeSpan? value, Func<TimeSpan?, ILocalSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }
}
