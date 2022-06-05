using System;

namespace SpawnThat.Core.Toml.Writers;

internal interface IValueWriter
{
    string Write(ITomlConfigEntry entry);
}

internal abstract class ValueWriter<T> : IValueWriter
{
    protected Type WriterType { get; }

    protected ValueWriter()
    {
        WriterType = typeof(T);
    }

    public string Write(ITomlConfigEntry entry)
    {
        if (entry is ITomlConfigEntry<T> supportedEntry)
        {
            return WriteInternal(supportedEntry);
        }
        else
        {
            throw new InvalidOperationException($"Unable to write config entry with type {typeof(T).Name} using writer {GetType().Name}.");
        }
    }

    protected abstract string WriteInternal(ITomlConfigEntry<T> entry);
}
