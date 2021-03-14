using System;
using System.Collections.Generic;

namespace Valheim.SpawnThat.ConfigurationCore
{
    [Serializable]
    public class ConfigurationGroup<TSection> : IHaveEntries where TSection : ConfigurationSection
    {
        public string GroupName { get; set; } = null;

        public Dictionary<string, TSection> Sections { get; set; } = null;

        public Dictionary<string, IConfigurationEntry> Entries { get; set; } = null;

        public ConfigurationGroup()
        {
        }
    }
}
