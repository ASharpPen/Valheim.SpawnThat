using System;

namespace SpawnThat.Core.Toml.Parsers;

internal class EnumParser<T> : ValueParser<T?>
    where T : struct, Enum
{
    protected override void ParseInternal(ITomlConfigEntry<T?> entry, string value)
    {
        if (Enum.TryParse<T>(value, true, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }

        entry.IsSet = false;
    }
}
