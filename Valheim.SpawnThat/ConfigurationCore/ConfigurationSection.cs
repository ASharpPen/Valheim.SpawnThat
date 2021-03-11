using System.Collections.Generic;

namespace Valheim.SpawnThat.ConfigurationCore
{
    public abstract class ConfigurationSection : IHaveEntries
    {
        public string SectionName { get; set; } = null;

        public Dictionary<string, IConfigurationEntry> Entries { get; set; } = null;
    }
}
