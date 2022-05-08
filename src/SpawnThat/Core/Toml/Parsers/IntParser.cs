namespace SpawnThat.Core.Toml.Parsers;

internal class IntParser : ValueParser<int?>
{
    protected override void ParseInternal(ITomlConfigEntry<int?> entry, string value)
    {
        if (int.TryParse(value, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }

        entry.IsSet = false;
    }
}
