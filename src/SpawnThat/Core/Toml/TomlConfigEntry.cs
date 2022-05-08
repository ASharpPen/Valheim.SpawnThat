using System;

namespace SpawnThat.Core.Toml;

public class TomlConfigEntry<T> : ITomlConfigEntry<T>
{
    internal TomlConfigEntry()
    { }

    public TomlConfigEntry(string name, T defaultValue, string description = null)
    {
        Name = name;
        DefaultValue = defaultValue;
        Description = description;
    }

    public string Name { get; set; }

    public Type SettingType { get; } = typeof(T);

    public T Value { get; set; }

    public T DefaultValue { get; set; }

    public string Description { get; set; }

    public bool IsSet { get; set; }
}
