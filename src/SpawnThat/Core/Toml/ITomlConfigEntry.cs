using System;

namespace SpawnThat.Core.Toml;

internal interface ITomlConfigEntry
{
    string Name { get; }

    string Description { get; }

    public Type SettingType { get; }

    public bool IsSet { get; set; }
}

internal interface ITomlConfigEntry<T> : ITomlConfigEntry
{
    T Value { get; set; }

    T DefaultValue { get; set; }
}

internal interface ITomlValueConfigEntry<T> : ITomlConfigEntry
    where T : struct
{
    T? Value { get; set; }

    T? DefaultValue { get; set; }
}