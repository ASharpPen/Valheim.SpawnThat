using System;

namespace SpawnThat.Core.Toml.Parsers;

internal interface IValueParser
{
    void Parse(string value, ITomlConfigEntry entry);
}

internal abstract class ValueParser<T> : IValueParser
{
    public void Parse(string value, ITomlConfigEntry entry)
    {
        if (entry is ITomlConfigEntry<T> supportedEntry)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                supportedEntry.IsSet = true;
            }
            else
            {
                ParseInternal(supportedEntry, value);
            }
        }
        else
        {
            throw new InvalidOperationException($"Unable to parse config entry with type {typeof(T).Name} using parser {GetType().Name}.");
        }
    }

    protected abstract void ParseInternal(ITomlConfigEntry<T> entry, string value);
}
