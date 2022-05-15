using System;

namespace SpawnThat.Core.Toml.Parsers;

internal class EnumParser<T> : ValueParser<T>
    where T : struct, Enum
{
    protected override void ParseInternal(ITomlConfigEntry<T> entry, TomlLine line)
    {
        if (Enum.TryParse<T>(line.Value, true, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }
        else
        {
            Log.LogWarning($"{line.FileName}, Line {line.LineNr}: Unable to parse '{line.Value}'. Verify spelling.");
            entry.IsSet = false;
        }
    }
}

internal class NullableEnumParser<T> : ValueParser<T?>
    where T : struct, Enum
{
    protected override void ParseInternal(ITomlConfigEntry<T?> entry, TomlLine line)
    {
        if (Enum.TryParse<T>(line.Value, true, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }
        else
        {
            Log.LogWarning($"{line.FileName}, Line {line.LineNr}: Unable to parse '{line.Value}'. Verify spelling.");
            entry.IsSet = false;
        }
    }
}
