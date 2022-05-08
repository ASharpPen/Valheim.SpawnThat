using System;
using System.Collections.Generic;
using SpawnThat.Utilities;

namespace SpawnThat.Core.Toml.Parsers;

internal class EnumListParser<T> : ValueParser<List<T>>
    where T : struct, Enum
{
    protected override void ParseInternal(ITomlConfigEntry<List<T>> entry, string value)
    {
        var items = value.SplitByComma();
        var results = new List<T>(items.Count);

        foreach (var itemValue in value.SplitByComma())
        {
            if (Enum.TryParse<T>(value, true, out var result))
            {
                results.Add(result);
            }
            else
            {
                entry.IsSet = false;
            }
        }

        entry.Value = results;
        entry.IsSet = true;
    }
}
