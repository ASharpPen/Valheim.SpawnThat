using System.Globalization;

namespace SpawnThat.Core.Toml.Parsers;

internal class FloatParser : ValueParser<float>
{
    protected override void ParseInternal(ITomlConfigEntry<float> entry, TomlLine line)
    {
        if (float.TryParse(line.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }
        else
        {
            Log.LogWarning($"{line.FileName}, Line {line.LineNr}: Unable to parse '{line.Value}'. Expected decimal number. Eg., 0.5 or 3.");
            entry.IsSet = false;
        }
    }
}

internal class NullableFloatParser : ValueParser<float?>
{
    protected override void ParseInternal(ITomlConfigEntry<float?> entry, TomlLine line)
    {
        if (float.TryParse(line.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }
        else
        {
            Log.LogWarning($"{line.FileName}, Line {line.LineNr}: Unable to parse '{line.Value}'. Expected decimal number. Eg., 0.5 or 3.");
            entry.IsSet = false;
        }
    }
}
