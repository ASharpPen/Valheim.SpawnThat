using System;
using System.Collections.Generic;

namespace SpawnThat.Spawners.DestructibleSpawner;

public static class IDestructibleSpawnBuilderSettingsExtensions
{
    public static IDestructibleSpawnBuilder WithSettings(this IDestructibleSpawnBuilder builder, DestructibleSpawnSettings settings)
    {
        settings.Enabled.Set(builder.SetEnabled);
        settings.TemplateEnabled.Set(builder.SetTemplateEnabled);
        settings.PrefabName.Set(builder.SetPrefabName);
        settings.SpawnWeight.Set(builder.SetSpawnWeight);
        settings.LevelMin.Set(builder.SetLevelMin);
        settings.LevelMax.Set(builder.SetLevelMax);

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

    private static void Set(this bool? value, Func<bool, IDestructibleSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this string value, Func<string, IDestructibleSpawnBuilder> apply)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            apply(value);
        }
    }

    private static void Set(this float? value, Func<float?, IDestructibleSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this int? value, Func<int?, IDestructibleSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set<T>(this List<T> value, Func<List<T>, IDestructibleSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Count > 0)
        {
            apply(value);
        }
    }
}
