using System.Globalization;

namespace SpawnThat.Core.Toml.Writers;

internal class IntWriter : ValueWriter<int>
{
    protected override string WriteInternal(ITomlConfigEntry<int> entry)
    {
        return entry.Value.ToString(CultureInfo.InvariantCulture);
    }
}

internal class NullableIntWriter : ValueWriter<int?>
{
    protected override string WriteInternal(ITomlConfigEntry<int?> entry)
    {
        if (entry.IsSet && entry.Value is not null)
        {
            return entry.Value.Value.ToString(CultureInfo.InvariantCulture);
        }
        else
        {
            return string.Empty;
        }
    }
}
