using System;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Configuration;

/// <summary>
/// Simple wrapper to detect if an option has been assigned or not.
/// 
/// This is intended to  help identify intentional null values, from simply never assigned ones.
/// </summary>
internal class BuilderOption<T>
{
    public T Value { get; set; }

    public BuilderOption(T value)
    {
        Value = value;
    }
}

/*
internal class BuilderOption<T>
{
    public bool IsSet { get; private set; }

    public T Value { get; private set; }

    public void Set(T value)
    {
        IsSet = true;
        Value = value;
    }
}
*/

internal static class BuilderOptionExtensions
{
    public static void DoIfSet<T>(this BuilderOption<T> option, Action<T> action)
    {
        if (option is not null)
        {
            action(option.Value);
        }
    }

    public static void DoIfSet<T>(this BuilderOption<T> option, Action<BuilderOption<T>> action)
    {
        if (option is not null)
        {
            action(option);
        }
    }

    public static void AssignIfSet<T>(this BuilderOption<T> source, ref BuilderOption<T> target)
    {
        if (source is not null)
        {
            target = source;
        }
    }
}