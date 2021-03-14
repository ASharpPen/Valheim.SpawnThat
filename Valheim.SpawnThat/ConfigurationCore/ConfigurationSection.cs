using System;
using System.Collections.Generic;

namespace Valheim.SpawnThat.ConfigurationCore
{
    [Serializable]
    public abstract class ConfigurationSection : IHaveEntries
    {
        public string SectionName { get; set; } = null;

        public Dictionary<string, IConfigurationEntry> Entries { get; set; } = null;
    }
}
