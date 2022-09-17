using System;

namespace SpawnThat.Spawners.LocalSpawner;

public static class ILocalSpawnBuilderSettingsExtensions
{
    public static ILocalSpawnBuilder WithSettings(this ILocalSpawnBuilder builder, LocalSpawnSettings settings)
    {
        settings.PrefabName.Set(builder.SetPrefabName);
        settings.Enabled.Set(x => builder.SetEnabled(x.Value));
        settings.SpawnInterval.SetNullable(builder.SetSpawnInterval);
        settings.MinLevel.Set(builder.SetMinLevel);
        settings.MaxLevel.Set(builder.SetMaxLevel);
        settings.SpawnDuringNight.Set(builder.SetSpawnDuringDay);
        settings.SpawnDuringNight.Set(builder.SetSpawnDuringNight);
        settings.SpawnInPlayerBase.Set(builder.SetSpawnInPlayerBase);
        settings.SetPatrolSpawn.Set(builder.SetPatrolSpawn);
        settings.LevelUpChance.Set(builder.SetLevelUpChance);
        settings.ConditionPlayerWithinDistance.Set(builder.SetConditionPlayerWithinDistance);
        settings.ConditionPlayerNoise.Set(builder.SetConditionPlayerNoise);

        foreach (var condition in settings.Conditions)
        {
            builder.SetCondition(condition);
        }

        foreach (var modifier in settings.Modifiers)
        {
            builder.SetModifier(modifier);
        }

        return builder;
    }

    private static void Set(this string value, Func<string, ILocalSpawnBuilder> apply)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            apply(value);
        }
    }

    private static void Set(this bool? value, Func<bool?, ILocalSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this int? value, Func<int?, ILocalSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this float? value, Func<float?, ILocalSpawnBuilder> apply)
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
