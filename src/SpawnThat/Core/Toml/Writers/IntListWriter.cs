using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HarmonyLib;

namespace SpawnThat.Core.Toml.Writers;

internal class IntListWriter : ValueWriter<List<int>>
{
    protected override string WriteInternal(ITomlConfigEntry<List<int>> entry)
    {
        if (entry.IsSet && entry.Value is not null)
        {
            return entry.Value.Select(x => x.ToString(CultureInfo.InvariantCulture)).Join();
        }
        else
        {
            return string.Empty;
        }
    }
}
