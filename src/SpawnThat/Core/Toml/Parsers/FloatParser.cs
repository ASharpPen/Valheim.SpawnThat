using System.Globalization;

namespace SpawnThat.Core.Toml.Parsers;

internal class FloatParser : ValueParser<float?>
{
    protected override void ParseInternal(ITomlConfigEntry<float?> entry, string value)
    {
        if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }

        entry.IsSet = false;
    }
}
