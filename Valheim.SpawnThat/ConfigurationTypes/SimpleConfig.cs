using System;
using Valheim.SpawnThat.ConfigurationCore;

namespace Valheim.SpawnThat.ConfigurationTypes
{
    [Serializable]
    public class SimpleConfig : ConfigurationGroup<SimpleConfigSection>
    {
        public ConfigurationEntry<string> PrefabName = new ConfigurationEntry<string>("Greydwarf", "Prefab name of entity to modify.");

        public ConfigurationEntry<bool> Enable = new ConfigurationEntry<bool>(true, "Enable/Disable this set of modifiers.");

        public ConfigurationEntry<float> SpawnMaxMultiplier = new ConfigurationEntry<float>(1, "Change maximum of total spawned entities. 2 means twice as many.");

        public ConfigurationEntry<float> GroupSizeMinMultiplier = new ConfigurationEntry<float>(1, "Change min number of entities that will spawn at once. 2 means twice as many.");

        public ConfigurationEntry<float> GroupSizeMaxMultiplier = new ConfigurationEntry<float>(1, "Change max number of entities that will spawn at once. 2 means twice as many.");

        public ConfigurationEntry<float> SpawnFrequencyMultiplier = new ConfigurationEntry<float>(1, "Change how often the game will try to spawn in new creatures.\nHigher means more often. 2 is twice as often, 0.5 is double the time between spawn checks.");
    }

    [Serializable]
    public class SimpleConfigSection : ConfigurationSection
    {
        
    }
}
