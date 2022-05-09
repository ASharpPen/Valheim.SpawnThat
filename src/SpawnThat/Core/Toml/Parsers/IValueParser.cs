using System;
using BepInEx;

namespace SpawnThat.Core.Toml.Parsers;

internal interface IValueParser
{
    void Parse(ITomlConfigEntry entry, TomlLine line);
}

internal abstract class ValueParser<T> : IValueParser
{
    public void Parse(ITomlConfigEntry entry, TomlLine line)
    {
        if (entry is ITomlConfigEntry<T> supportedEntry)
        {
            if (string.IsNullOrWhiteSpace(line.Value))
            {
                supportedEntry.IsSet = true;
            }
            else
            {
                ParseInternal(supportedEntry, line);
            }
        }
        else
        {
            throw new InvalidOperationException($"Unable to parse config entry with type {typeof(T).Name} using parser {GetType().Name}.");
        }
    }

    protected abstract void ParseInternal(ITomlConfigEntry<T> entry, TomlLine line);
}
