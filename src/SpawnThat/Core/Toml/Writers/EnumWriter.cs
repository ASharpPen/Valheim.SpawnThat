using System;

namespace SpawnThat.Core.Toml.Writers;

internal class EnumWriter<T> : ValueWriter<T>
    where T : struct, Enum
{
    protected override string WriteInternal(ITomlConfigEntry<T> entry)
    {
        return entry.Value.ToString();
    }
}

internal class NullableEnumWriter<T> : ValueWriter<T?>
    where T : struct, Enum
{
    protected override string WriteInternal(ITomlConfigEntry<T?> entry)
    {
        if (entry.IsSet && entry.Value is not null)
        {
            return entry.Value.Value.ToString();
        }
        else
        {
            return string.Empty;
        }
    }
}