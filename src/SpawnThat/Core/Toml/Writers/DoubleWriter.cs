using System.Globalization;

namespace SpawnThat.Core.Toml.Writers;

internal class DoubleWriter : ValueWriter<double>
{
    protected override string WriteInternal(ITomlConfigEntry<double> entry)
    {
        return entry.Value.ToString(CultureInfo.InvariantCulture);
    }
}

internal class NullableDoubleWriter : ValueWriter<double?>
{
    protected override string WriteInternal(ITomlConfigEntry<double?> entry)
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
