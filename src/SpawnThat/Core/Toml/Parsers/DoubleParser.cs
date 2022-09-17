using System.Globalization;

namespace SpawnThat.Core.Toml.Parsers;

internal class DoubleParser : ValueParser<double>
{
    protected override void ParseInternal(ITomlConfigEntry<double> entry, TomlLine line)
    {
        if (double.TryParse(line.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }
        else
        {
            Log.LogWarning($"{line.FileName}, Line {line.LineNr}: Unable to parse '{line.Value}' as a double. Verify spelling. Valid example values are \"0.5\" and \"123\".");
            entry.IsSet = false;
        }
    }
}

internal class NullableDoubleParser : ValueParser<double?>
{
    protected override void ParseInternal(ITomlConfigEntry<double?> entry, TomlLine line)
    {
        if (double.TryParse(line.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }
        else
        {
            Log.LogWarning($"{line.FileName}, Line {line.LineNr}: Unable to parse '{line.Value}' as a double. Verify spelling. Valid example values are \"0.5\" and \"123\".");
            entry.IsSet = false;
        }
    }
}
