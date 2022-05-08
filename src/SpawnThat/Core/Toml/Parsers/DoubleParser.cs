using System.Globalization;

namespace SpawnThat.Core.Toml.Parsers;

internal class DoubleParser : ValueParser<double?>
{
    protected override void ParseInternal(ITomlConfigEntry<double?> entry, string value)
    {
        if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }

        entry.IsSet = false;
    }
}
