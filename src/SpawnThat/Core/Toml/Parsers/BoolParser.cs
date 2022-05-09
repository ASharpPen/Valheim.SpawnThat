namespace SpawnThat.Core.Toml.Parsers;

internal class BoolParser : ValueParser<bool?>
{
    protected override void ParseInternal(ITomlConfigEntry<bool?> entry, TomlLine line)
    {
        if (bool.TryParse(line.Value, out var result))
        {
            entry.Value = result;
            entry.IsSet = true;
        }
        else
        {
            Log.LogWarning($"[Line {line.LineNr}]: Unable to parse '{line.Value}' as a boolean. Verify spelling. Valid values are \"true\" and \"false\".");
            entry.IsSet = false;
        }
    }
}
