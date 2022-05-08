namespace SpawnThat.Core.Toml.Parsers;

internal class StringParser : ValueParser<string>
{
    protected override void ParseInternal(ITomlConfigEntry<string> entry, string value)
    {
        entry.Value = value.Trim();
        entry.IsSet = true;
    }
}
