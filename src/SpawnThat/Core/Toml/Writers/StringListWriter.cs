using System.Collections.Generic;
using System.Linq;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Core.Toml.Writers;

internal class StringListWriter : ValueWriter<List<string>>
{
    protected override string WriteInternal(ITomlConfigEntry<List<string>> entry)
    {
        if (entry.IsSet && entry.Value is not null)
        {
            return entry.Value.Join();
        }
        else
        {
            return string.Empty;
        }
    }
}
