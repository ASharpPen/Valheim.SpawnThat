using System;
using System.Collections.Generic;
using SpawnThat.Utilities;

namespace SpawnThat.Core.Toml.Parsers;

internal class EnumListParser<T> : ValueParser<List<T>>
    where T : struct, Enum
{
    protected override void ParseInternal(ITomlConfigEntry<List<T>> entry, TomlLine line)
    {
        var items = line.Value.SplitByComma();
        var results = new List<T>(items.Count);

        foreach (var itemValue in items)
        {
            if (Enum.TryParse<T>(itemValue, true, out var result))
            {
                results.Add(result);
            }
            else
            {
                Log.LogWarning($"{line.FileName}, Line {line.LineNr}: Unable to parse '{itemValue}'. Verify spelling.");
            }
        }

        if (items.Count > 0 && 
            results.Count == 0)
        {
            // Everything failed. Skip this one.
            entry.IsSet = false;
            return;
        }

        entry.Value = results;
        entry.IsSet = true;
    }
}
