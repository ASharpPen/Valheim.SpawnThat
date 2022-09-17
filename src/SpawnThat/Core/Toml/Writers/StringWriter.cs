namespace SpawnThat.Core.Toml.Writers;

internal class StringWriter : ValueWriter<string>
{
    protected override string WriteInternal(ITomlConfigEntry<string> entry)
    {
        return entry.Value ?? string.Empty;
    }
}
