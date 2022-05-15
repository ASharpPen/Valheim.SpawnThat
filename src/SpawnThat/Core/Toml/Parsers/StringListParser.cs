using System.Collections.Generic;
using SpawnThat.Utilities;

namespace SpawnThat.Core.Toml.Parsers;

internal class StringListParser : ValueParser<List<string>>
{
    protected override void ParseInternal(ITomlConfigEntry<List<string>> entry, TomlLine line)
    {
        entry.Value = line.Value.SplitByComma();
        entry.IsSet = true;
    }
}
