namespace SpawnThat.Core.Toml.Parsers;

internal class StringParser : ValueParser<string>
{
    protected override void ParseInternal(ITomlConfigEntry<string> entry, TomlLine line)
    {
        entry.Value = line.Value.Trim();
        entry.IsSet = true;
    }
}
