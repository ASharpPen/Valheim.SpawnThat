using System;
using Valheim.SpawnThat.Core.Configuration;

namespace Valheim.SpawnThat.Configuration.ConfigTypes
{
    [Serializable]
    public class SpawnSystemConfigurationFile : ConfigWithSubsections<SpawnSystemConfiguration>, IConfigFile
    {
        protected override SpawnSystemConfiguration InstantiateSubsection(string subsectionName)
        {
            return new SpawnSystemConfiguration();
        }
    }

    [Serializable]
    public class SpawnSystemConfiguration : ConfigWithSubsections<SpawnConfiguration>
    {
        protected override SpawnConfiguration InstantiateSubsection(string subsectionName)
        {
            return new SpawnConfiguration();
        }
    }

    [Serializable]
    public class SpawnConfiguration : ConfigWithSubsections<Config>
    {
        protected override Config InstantiateSubsection(string subsectionName)
        {
            Config newModConfig = null;

            if (subsectionName == SpawnSystemConfigCLLC.ModName.Trim().ToUpperInvariant())
            {
                newModConfig = new SpawnSystemConfigCLLC();
            }
            else if(subsectionName == SpawnSystemConfigMobAI.ModName.Trim().ToUpperInvariant())
            {
                newModConfig = new SpawnSystemConfigMobAI();
            }
            else if(subsectionName == SpawnSystemConfigEpicLoot.ModName.Trim().ToUpperInvariant())
            {
                newModConfig = new SpawnSystemConfigEpicLoot();
            }

            return newModConfig;
        }

        private int? index = null;

        public int Index
        {
            get
            {
                if (index.HasValue)
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

        public ConfigurationEntry<string> TemplateId = new ConfigurationEntry<string>("", "Technical setting intended for cross-mod identification of mobs spawned by this template. Sets a custom identifier which will be assigned to the spawned mobs ZDO as 'ZDO.Set(\"spawn_template_id\", TemplateIdentifier)'.");

        public ConfigurationEntry<string> Name = new ConfigurationEntry<string>("My spawner", "Just a field for naming the configuration entry.");

        public ConfigurationEntry<bool> Enabled = new ConfigurationEntry<bool>(true, "Enable/disable this entry.");

        public ConfigurationEntry<string> Biomes = new ConfigurationEntry<string>("", "Biomes in which entity can spawn. Leave empty for all.");

        //public ConfigurationEntry<bool> DriveInward = new ConfigurationEntry<bool>(false, "Mobs always spawn towards the world edge from player.");

        //Bound to the spawner itself. Need to transpile in a change for this to work.
        //public ConfigurationEntry<float> LevelUpChance = new ConfigurationEntry<float>(10, "Chance to increase level above min. This is run multiple times. 100 is 100%.\nEg. if Chance is 10, LevelMin is 1 and LevelMax is 3, the game will have a 10% to become level 2. The game will then run an additional 10% check for increasing to level 3.");

        public ConfigurationEntry<float> ConditionDistanceToCenterMin = new ConfigurationEntry<float>(0, "Minimum distance to center for configuration to apply.");

        public ConfigurationEntry<float> ConditionDistanceToCenterMax = new ConfigurationEntry<float>(0, "Maximum distance to center for configuration to apply. 0 means limitless.");

        public ConfigurationEntry<float> ConditionWorldAgeDaysMin = new ConfigurationEntry<float>(0, "Minimum world age in in-game days for this configuration to apply.");

        public ConfigurationEntry<float> ConditionWorldAgeDaysMax = new ConfigurationEntry<float>(0, "Maximum world age in in-game days for this configuration to apply. 0 means no max.");

        public ConfigurationEntry<float> DistanceToTriggerPlayerConditions = new ConfigurationEntry<float>(100, "Distance of player to spawner, for player to be included in player based checks such as ConditionNearbyPlayersCarryValue.");

        public ConfigurationEntry<int> ConditionNearbyPlayersCarryValue = new ConfigurationEntry<int>(0, "Checks if nearby players have a combined value in inventory above this condition.\nEg. If set to 100, entry will only activate if nearby players have more than 100 worth of values combined.");

        public ConfigurationEntry<string> ConditionNearbyPlayerCarriesItem = new ConfigurationEntry<string>("", "Checks if nearby players have any of the listed item prefab names in inventory.\nEg. IronScrap, DragonEgg");

        public ConfigurationEntry<float> ConditionNearbyPlayersNoiseThreshold = new ConfigurationEntry<float>(0, "Checks if any nearby players have accumulated noise at or above the threshold.");

        public ConfigurationEntry<string> ConditionNearbyPlayersStatus = new ConfigurationEntry<string>("", "Checks if any nearbly players have any of the listed status effects.\nEg. Wet, Burning");

        public ConfigurationEntry<string> RequiredNotGlobalKey = new ConfigurationEntry<string>("", "Array of global keys which disable the spawning of this entity if any are detected.\nEg. defeated_bonemass,KilledTroll");

        public ConfigurationEntry<string> SetFaction = new ConfigurationEntry<string>("", "Assign a specific faction to spawn. If empty uses default.");

        public ConfigurationEntry<bool> SetRelentless = new ConfigurationEntry<bool>(false, "When true, forces mob AI to always be alerted.");

        public ConfigurationEntry<bool> SetTryDespawnOnConditionsInvalid = new ConfigurationEntry<bool>(false, "When true, mob will try to run away and despawn when spawn conditions become invalid.\nEg. if spawning only during night, it will run away and despawn at night. Currently this only take into account conditions for daytime and environment.");

        public ConfigurationEntry<bool> SetTryDespawnOnAlert = new ConfigurationEntry<bool>(false, "When true, mob will try to run away and despawn when alerted.");

        public ConfigurationEntry<bool> SetTamed = new ConfigurationEntry<bool>(false, "When true, mob will be set to tamed status on spawn.");

        public ConfigurationEntry<bool> SetTamedCommandable = new ConfigurationEntry<bool>(false, "Experimental. When true, will set mob as commandable when tamed. When false, whatever was default for the creature is used. Does not always seem to work for creatures not tameable in vanilla.");

        public ConfigurationEntry<string> ConditionLocation = new ConfigurationEntry<string>("", "Array of locations in which this spawn is enabled. If empty, allows all.\nEg. Runestone_Boars, FireHole");

        public ConfigurationEntry<float> ConditionAreaSpawnChance = new ConfigurationEntry<float>(100, "Chance for spawn to spawn at all in the area. The chance will be rolled once for the area. Range is 0 to 100. Eg. if a whole area of BlackForest rolls higher than the indicated chance, this spawn template will never be active in that forest. Another BlackForest will have another roll however, that may activate this template there. Chance is rolled based on world seed, area id and template index.");

        public ConfigurationEntry<string> ConditionAreaIds = new ConfigurationEntry<string>("", "Advanced feature. List of area id's in which the template is valid. Note: If ConditionSpawnChanceInArea is not 100 or disabled, it will still roll area chance.\nEg. 1, 123, 543");

        public ConfigurationEntry<float> ConditionNearbyPlayerWithHealthMin = new ConfigurationEntry<float>(0, "Checks if any nearby player has above the required mininum of health.");

        public ConfigurationEntry<float> ConditionNearbyPlayerWithHealthMax = new ConfigurationEntry<float>(0, "Checks if any nearby player has below the required maximum of health. 0 means no max.");
        
        public ConfigurationEntry<float> ConditionNearbyPlayerWithHealthPercentMin = new ConfigurationEntry<float>(0, "Checks if any nearby player has above the required mininum of health percentage.");
        
        public ConfigurationEntry<float> ConditionNearbyPlayerWithHealthPercentMax = new ConfigurationEntry<float>(0, "Checks if any nearby player has below the required maximum of health percentage. 0 means no max.");

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

        public ConfigurationEntry<string> RequiredGlobalKey = new ConfigurationEntry<string>("", "Required global key to spawn.\nEg. defeated_bonemass");

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

    [Serializable]
    public class SpawnSystemConfigCLLC : Config
    {
        public const string ModName = "CreatureLevelAndLootControl";

        public ConfigurationEntry<int> ConditionWorldLevelMin = new ConfigurationEntry<int>(-1, "Minimum CLLC world level for spawn to activate. Negative value disables this condition.");

        public ConfigurationEntry<int> ConditionWorldLevelMax = new ConfigurationEntry<int>(-1, "Maximum CLLC world level for spawn to active. Negative value disables this condition.");

        public ConfigurationEntry<string> SetInfusion = new ConfigurationEntry<string>("", "Assigns the specified infusion to creature spawned. Ignored if empty.");

        public ConfigurationEntry<string> SetExtraEffect = new ConfigurationEntry<string>("", "Assigns the specified effect to creature spawned. Ignored if empty.");

        public ConfigurationEntry<string> SetBossAffix = new ConfigurationEntry<string>("", "Assigns the specified boss affix to creature spawned. Only works for the default 5 bosses. Ignored if empty.");

        public ConfigurationEntry<bool> UseDefaultLevels = new ConfigurationEntry<bool>(false, "Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control.");
    }

    [Serializable]
    public class SpawnSystemConfigMobAI : Config
    {
        public const string ModName = "MobAI";

        public ConfigurationEntry<string> SetAI = new ConfigurationEntry<string>("", "Name of MobAI to register for spawn. Eg. the defaults 'Fixer' and 'Worker'.");

        public ConfigurationFileEntry AIConfigFile = new ConfigurationFileEntry("", "Configuration file to use for the SetAI. Eg. 'MyFixerConfig.json', can include path, but will always start searching from config folder. See MobAI documentation for file setup.");
    }

    public class SpawnSystemConfigEpicLoot: Config
    {
        public const string ModName = "EpicLoot";

        public ConfigurationEntry<string> ConditionNearbyPlayerCarryItemWithRarity = new ConfigurationEntry<string>("", "Checks if nearby players have any items of the listed rarities.\nEg. Magic, Legendary");

        public ConfigurationEntry<string> ConditionNearbyPlayerCarryLegendaryItem = new ConfigurationEntry<string>("", "Checks if nearby players have any of the listed epic loot legendary id's in inventory.\nEg. HeimdallLegs, RagnarLegs");
    }
}
