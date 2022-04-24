using System;
using System.Collections.Generic;

namespace SpawnThat.Spawners.DestructibleSpawner;

public static class IDestructibleSpawnerBuilderSettingsExtensions
{
    public static IDestructibleSpawnerBuilder WithSettings(this IDestructibleSpawnerBuilder builder, DestructibleSpawnerSettings settings)
    {
        settings.LevelUpChance.Set(builder.SetLevelUpChance);
        settings.SpawnInterval.Set(builder.SetSpawnInterval);
        settings.SetPatrol.Set(builder.SetPatrol);
        settings.ConditionPlayerWithinDistance.Set(builder.SetConditionPlayerWithinDistance);
        settings.ConditionMaxCloseCreatures.Set(builder.SetConditionMaxCloseCreatures);
        settings.ConditionMaxCreatures.Set(builder.SetConditionMaxCreatures);
        settings.DistanceConsideredClose.Set(builder.SetDistanceConsideredClose);
        settings.DistanceConsideredFar.Set(builder.SetDistanceConsideredFar);
        settings.OnGroundOnly.Set(builder.SetOnGroundOnly);

        if (settings.RemoveNotConfiguredSpawns is not null)
        {
            builder.SetRemoveNotConfiguredSpawns(settings.RemoveNotConfiguredSpawns.Value);
        }

        if (settings.Identifiers is not null)
        {
            foreach (var identifier in settings.Identifiers)
            {
                if (identifier is null)
                {
                    continue;
                }

                builder.SetIdentifier(identifier);
            }
        }

        if (settings.Spawns is not null)
        {
            foreach (var spawn in settings.Spawns)
            {
                if (spawn.Value is null)
                {
                    continue;
                }

                builder.GetSpawnBuilder(spawn.Key)
                    .WithSettings(spawn.Value);
            }
        }

        return builder;
    }

    private static void Set(this bool? value, Func<bool?, IDestructibleSpawnerBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this string value, Func<string, IDestructibleSpawnerBuilder> apply)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            apply(value);
        }
    }

    private static void Set(this float? value, Func<float?, IDestructibleSpawnerBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this int? value, Func<int?, IDestructibleSpawnerBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this TimeSpan? value, Func<TimeSpan?, IDestructibleSpawnerBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set<T>(this List<T> value, Func<List<T>, IDestructibleSpawnerBuilder> apply)
    {
        if (value is not null &&
            value.Count > 0)
        {
            apply(value);
        }
    }
}
