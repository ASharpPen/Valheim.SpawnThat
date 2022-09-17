using System.Collections.Generic;

namespace SpawnThat.Core.Toml;

internal interface IHaveSubsections
{
    TomlConfig GetSubsection(string subsectionName);

    List<KeyValuePair<string, TomlConfig>> GetSubsections();
}
