using System;
using System.Collections.Generic;

namespace SpawnThat.Spawners.SpawnAreaSpawner;

public static class ISpawnAreaSpawnBuilderSettingsExtensions
{
    public static ISpawnAreaSpawnBuilder WithSettings(this ISpawnAreaSpawnBuilder builder, SpawnAreaSpawnSettings settings)
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

    private static void Set(this bool? value, Func<bool, ISpawnAreaSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this string value, Func<string, ISpawnAreaSpawnBuilder> apply)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            apply(value);
        }
    }

    private static void Set(this float? value, Func<float?, ISpawnAreaSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set(this int? value, Func<int?, ISpawnAreaSpawnBuilder> apply)
    {
        if (value is not null)
        {
            apply(value.Value);
        }
    }

    private static void Set<T>(this List<T> value, Func<List<T>, ISpawnAreaSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Count > 0)
        {
            apply(value);
        }
    }
}
