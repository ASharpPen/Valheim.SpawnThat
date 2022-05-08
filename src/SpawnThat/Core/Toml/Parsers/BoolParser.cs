namespace SpawnThat.Core.Toml.Parsers;

internal class BoolParser : ValueParser<bool?>
{
    protected override void ParseInternal(ITomlConfigEntry<bool?> entry, string value)
    {
        if (bool.TryParse(value, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }

        entry.IsSet = false;
    }
}
