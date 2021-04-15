using System;
using System.Collections.Generic;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat.Multiplayer
{
    [Serializable]
    internal class ConfigurationPackage
    {
        public GeneralConfig GeneralConfig;

        public List<SimpleConfig> SimpleConfig;

        public SpawnSystemConfigurationAdvanced SpawnSystemConfig;

        public Dictionary<string, CreatureSpawnerConfigurationAdvanced> CreatureSpawnerConfig;

        public ConfigurationPackage(){ }

        public ConfigurationPackage(
            GeneralConfig generalConfig,
            List<SimpleConfig> simpleConfig,
            SpawnSystemConfigurationAdvanced spawnSystemConfig,
            Dictionary<string, CreatureSpawnerConfigurationAdvanced> creatureSpawnerConfig)
        {
            GeneralConfig = generalConfig;
            SimpleConfig = simpleConfig;
            SpawnSystemConfig = spawnSystemConfig;
            CreatureSpawnerConfig = creatureSpawnerConfig;
        }
    }
}
