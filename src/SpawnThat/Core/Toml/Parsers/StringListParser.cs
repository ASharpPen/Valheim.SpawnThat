using System.Collections.Generic;
using SpawnThat.Utilities;

namespace SpawnThat.Core.Toml.Parsers;

internal class StringListParser : ValueParser<List<string>>
{
    protected override void ParseInternal(ITomlConfigEntry<List<string>> entry, string value)
    {
        entry.Value = value.SplitByComma();
        entry.IsSet = true;
    }
}
