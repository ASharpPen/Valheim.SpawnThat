using System;
using Valheim.SpawnThat.ConfigurationCore;

namespace Valheim.SpawnThat.ConfigurationTypes
{
    [Serializable]
    public class SpawnSystemConfigurationAdvanced : ConfigurationGroup<SpawnConfiguration>
    {
    }

    [Serializable]
    public class SpawnConfiguration : ConfigurationSection
    {
        private int? index = null;

        public int Index
        {
            get
            {
                if(index.HasValue)
                {
                    return index.Value;
                }

                if (int.TryParse(SectionName, out int sectionIndex) && sectionIndex >= 0)
                {
                    index = sectionIndex;
                }
                else
                {
                    index = int.MaxValue;
                }

                return index.Value;
            }
        }

        public ConfigurationEntry<string> Name = new ConfigurationEntry<string>("My spawner", "Just a field for naming the configuration entry.");

        public ConfigurationEntry<bool> Enabled = new ConfigurationEntry<bool>(true, "Enable/disable this entry.");

        public ConfigurationEntry<string> Biomes = new ConfigurationEntry<string>("", "Biomes in which entity can spawn. Leave empty for all.");

        //public ConfigurationEntry<bool> DriveInward = new ConfigurationEntry<bool>(false, "Mobs always spawn on towards the world edge from player.");

        //Bound to the spawner itself. Need to transpile in a change for this to work.
        //public ConfigurationEntry<float> LevelUpChance = new ConfigurationEntry<float>(10, "Chance to increase level above min. This is run multiple times. 100 is 100%.\nEg. if Chance is 10, LevelMin is 1 and LevelMax is 3, the game will have a 10% to become level 2. The game will then run an additional 10% check for increasing to level 3.");

        #region Default Configuration Options

        public ConfigurationEntry<string> PrefabName = new ConfigurationEntry<string>("Deer", "Prefab name of the entity to spawn.");

        public ConfigurationEntry<bool> HuntPlayer = new ConfigurationEntry<bool>(false, "Sets AI to hunt a player target.");

        public ConfigurationEntry<int> MaxSpawned = new ConfigurationEntry<int>(1, "Maximum entities of type spawned in area.");

        public ConfigurationEntry<float> SpawnInterval = new ConfigurationEntry<float>(90, "Seconds between spawn checks.");

        public ConfigurationEntry<float> SpawnChance = new ConfigurationEntry<float>(100, "Chance to spawn per check. Range 0 to 100.");

        public ConfigurationEntry<int> LevelMin = new ConfigurationEntry<int>(1, "Minimum level to spawn.");

        public ConfigurationEntry<int> LevelMax = new ConfigurationEntry<int>(1, "Maximum level to spawn.");

        public ConfigurationEntry<float> LevelUpMinCenterDistance = new ConfigurationEntry<float>(0, "Minimum distance from world center, to allow higher than min level.");

        public ConfigurationEntry<float> SpawnDistance = new ConfigurationEntry<float>(0, "Minimum distance to another entity.");

        public ConfigurationEntry<float> SpawnRadiusMin = new ConfigurationEntry<float>(0, "Minimum spawn radius.");

        public ConfigurationEntry<float> SpawnRadiusMax = new ConfigurationEntry<float>(0, "Maximum spawn radius.");

        public ConfigurationEntry<string> RequiredGlobalKey = new ConfigurationEntry<string>("", "Required global key to spawn.\tEg. defeated_bonemass");

        public ConfigurationEntry<string> RequiredEnvironments = new ConfigurationEntry<string>("", "Array (separate by comma) of environments required to spawn in.\tEg. Misty, Thunderstorm. Leave empty to allow all.");

        public ConfigurationEntry<int> GroupSizeMin = new ConfigurationEntry<int>(1, "Minimum count to spawn at per check.");

        public ConfigurationEntry<int> GroupSizeMax = new ConfigurationEntry<int>(1, "Maximum count to spawn at per check.");

        public ConfigurationEntry<float> GroupRadius = new ConfigurationEntry<float>(3, "Size of circle to spawn group inside.");

        public ConfigurationEntry<float> GroundOffset = new ConfigurationEntry<float>(0.5f, "Offset to ground to spawn at.");

        public ConfigurationEntry<bool> SpawnDuringDay = new ConfigurationEntry<bool>(true, "Toggles spawning at day.");

        public ConfigurationEntry<bool> SpawnDuringNight = new ConfigurationEntry<bool>(true, "Toggles spawning at night.");

        public ConfigurationEntry<float> ConditionAltitudeMin = new ConfigurationEntry<float>(-1000, "Minimum altitude (distance to water surface) to spawn in.");

        public ConfigurationEntry<float> ConditionAltitudeMax = new ConfigurationEntry<float>(1000, "Maximum altitude (distance to water surface) to spawn in.");

        public ConfigurationEntry<float> ConditionTiltMin = new ConfigurationEntry<float>(0, "Minium tilt of terrain to spawn in.");

        public ConfigurationEntry<float> ConditionTiltMax = new ConfigurationEntry<float>(35, "Maximum tilt of terrain to spawn in.");

        public ConfigurationEntry<bool> SpawnInForest = new ConfigurationEntry<bool>(true, "Toggles spawning in forest.");

        public ConfigurationEntry<bool> SpawnOutsideForest = new ConfigurationEntry<bool>(true, "Toggles spawning outside of forest.");

        public ConfigurationEntry<float> OceanDepthMin = new ConfigurationEntry<float>(0, "Minimum ocean depth to spawn in. Ignored if min == max.");

        public ConfigurationEntry<float> OceanDepthMax = new ConfigurationEntry<float>(0, "Maximum ocean depth to spawn in. Ignored if min == max.");

        #endregion
    }
}
