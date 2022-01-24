using System;
using SpawnThat.Core.Configuration;

namespace SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx
{
    [Serializable]
    public class SimpleConfigurationFile : ConfigWithSubsections<SimpleConfig>, IConfigFile
    {
        protected override SimpleConfig InstantiateSubsection(string subsectionName)
        {
            return new SimpleConfig();
        }
    }

    [Serializable]
    public class SimpleConfig : Config
    {
        public ConfigurationEntry<string> PrefabName = new("Greydwarf", "Prefab name of entity to modify.");

        public ConfigurationEntry<bool> Enable = new(true, "Enable/Disable this set of modifiers.");

        public ConfigurationEntry<float> SpawnMaxMultiplier = new(1, "Change maximum of total spawned entities. 2 means twice as many.");

        public ConfigurationEntry<float> GroupSizeMinMultiplier = new(1, "Change min number of entities that will spawn at once. 2 means twice as many.");

        public ConfigurationEntry<float> GroupSizeMaxMultiplier = new(1, "Change max number of entities that will spawn at once. 2 means twice as many.");

        public ConfigurationEntry<float> SpawnFrequencyMultiplier = new(1, "Change how often the game will try to spawn in new creatures.\nHigher means more often. 2 is twice as often, 0.5 is double the time between spawn checks.");
    }
}
