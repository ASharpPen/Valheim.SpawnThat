using System.Collections.Generic;

namespace Valheim.SpawnThat.ConfigurationCore
{
    public interface IHaveEntries
    {
        Dictionary<string, IConfigurationEntry> Entries { get; set; }
    }
}
