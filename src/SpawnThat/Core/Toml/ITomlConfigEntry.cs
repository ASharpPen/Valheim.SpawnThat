using System;

namespace SpawnThat.Core.Toml;

public interface ITomlConfigEntry
{
    string Name { get; }

    string Description { get; }

    public Type SettingType { get; }

    public bool IsSet { get; set; }
}

public interface ITomlConfigEntry<T> : ITomlConfigEntry
{
    T Value { get; set; }

    T DefaultValue { get; set; }
}

public interface ITomlValueConfigEntry<T> : ITomlConfigEntry
    where T : struct
{
    T? Value { get; set; }

    T? DefaultValue { get; set; }
}