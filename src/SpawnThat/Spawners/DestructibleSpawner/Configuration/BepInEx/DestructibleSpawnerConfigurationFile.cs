using SpawnThat.Core.Configuration;
using SpawnThat.Utilities.Enums;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration.BepInEx;

internal class DestructibleSpawnerConfigurationFile
    : ConfigWithSubsections<DestructibleSpawnerConfig>
    , IConfigFile
{
    protected override DestructibleSpawnerConfig InstantiateSubsection(string subsectionName)
    {
        return new DestructibleSpawnerConfig();
    }

}

internal class DestructibleSpawnerConfig
    : ConfigWithSubsections<DestructibleSpawnConfig>
{
    protected override DestructibleSpawnConfig InstantiateSubsection(string subsectionName)
    {
        return new DestructibleSpawnConfig();
    }

    #region Identifiers

    public ConfigurationEntry<string> IdentifyByName = new("", "Name of spawner prefab to match config with. Ignored if empty.");
    public ConfigurationEntry<string> IdentifyByBiome = new("", "Biomes in which to match spawner with config. Ignored if empty.");
    public ConfigurationEntry<string> IdentifyByLocation = new("", "Locations in which to match spawner with config. Ignored if empty.");
    public ConfigurationEntry<string> IdentifyByRoom = new("", "Rooms (eg., dungeons, camp buildings) in which to match spawner with config. Ignored if empty.");

    #endregion

    #region Default Settings

    public ConfigurationEntry<float> LevelUpChance = new(15, "Chance to level up from MinLevel. Range 0 to 100.");

    public ConfigurationEntry<float> SpawnInterval = new(10, "Seconds between spawn checks.");

    public ConfigurationEntry<bool> SetPatrol = new(false, "Sets if spawn should patrol its spawn point.");

    public ConfigurationEntry<float> ConditionPlayerWithinDistance = new(60, "Minimum distance to player for enabling spawn.");

    public ConfigurationEntry<int> ConditionMaxCloseCreatures = new(3, "Sets maximum number of creatures within DistanceConsideredClose, for spawner to be active.");

    public ConfigurationEntry<int> ConditionMaxCreatures = new(100, "Sets maximum number of creatures currently loaded, for spawner to be active.");

    public ConfigurationEntry<float> DistanceConsideredClose = new(20, "Distance within which another entity is counted as being close to spawner.");

    public ConfigurationEntry<float> DistanceConsideredFar = new(1000, "Distance within which another entity is counted as being far to spawner.");

    public ConfigurationEntry<bool> OnGroundOnly = new(false, "Only spawn if spawn point is on the ground (ie., not in a dungeon) and open to the sky.");

    #endregion

    #region Custom Settings

    //public ConfigurationEntry<bool> Enabled = new(true, "")

    #endregion
}

internal class DestructibleSpawnConfig : ConfigWithSubsections<Config>
{
    protected override Config InstantiateSubsection(string subsectionName)
    {
        Config newModConfig = null;

        if (subsectionName == DestructibleSpawnConfigCLLC.ModName.Trim().ToUpperInvariant())
        {
            newModConfig = new DestructibleSpawnConfigCLLC();
        }
        else if (subsectionName == DestructibleSpawnConfigMobAI.ModName.Trim().ToUpperInvariant())
        {
            newModConfig = new DestructibleSpawnConfigMobAI();
        }
        else if (subsectionName == DestructibleSpawnConfigEpicLoot.ModName.Trim().ToUpperInvariant())
        {
            newModConfig = new DestructibleSpawnConfigEpicLoot();
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

    public ConfigurationEntry<bool> Enabled = new(true, "Toggles this template. If disabled, this spawn entry will never be selected for spawning. Can be used to disable existing spawn entries.");

    public ConfigurationEntry<bool> TemplateEnabled = new(true, "Toggles this configuration on / off. If disabled, template will be ignored. Cannot be used to disable existing spawn entries.");

    public ConfigurationEntry<string> PrefabName = new("", "Prefab name of entity to spawn.");

    public ConfigurationEntry<float> SpawnWeight = new(1, "Sets spawn weight. Destructible spawners choose their next spawn by a weighted random of all their possible spawns.\nIncreasing weight, means an increased chance that this particular spawn will be selected for spawning.");

    public ConfigurationEntry<int> LevelMin = new(1, "Minimum level to spawn at.");

    public ConfigurationEntry<int> LevelMax = new(1, "Maximum level to spawn at.");

    #region Conditions
    public ConfigurationEntry<float> ConditionDistanceToCenterMin = new(0, "Minimum distance to center for configuration to apply.");

    public ConfigurationEntry<float> ConditionDistanceToCenterMax = new(0, "Maximum distance to center for configuration to apply. 0 means limitless.");

    public ConfigurationEntry<int> ConditionWorldAgeDaysMin = new(0, "Minimum world age in in-game days for this configuration to apply.");

    public ConfigurationEntry<int> ConditionWorldAgeDaysMax = new(0, "Maximum world age in in-game days for this configuration to apply. 0 means no max.");

    public ConfigurationEntry<float> DistanceToTriggerPlayerConditions = new(100, "Distance of player to spawner, for player to be included in player based checks such as ConditionNearbyPlayersCarryValue.");

    public ConfigurationEntry<int> ConditionNearbyPlayersCarryValue = new(0, "Checks if nearby players have a combined value in inventory above this condition.\nEg. If set to 100, entry will only activate if nearby players have more than 100 worth of values combined.");

    public ConfigurationEntry<string> ConditionNearbyPlayerCarriesItem = new("", "Checks if nearby players have any of the listed item prefab names in inventory.\nEg. IronScrap, DragonEgg");

    public ConfigurationEntry<float> ConditionNearbyPlayersNoiseThreshold = new(0, "Checks if any nearby players have accumulated noise at or above the threshold.");

    public ConfigurationEntry<string> ConditionNearbyPlayersStatus = new("", "Checks if any nearbly players have any of the listed status effects.\nEg. Wet, Burning");

    public ConfigurationEntry<float> ConditionAreaSpawnChance = new(100, "Chance for spawn to spawn at all in the area. The chance will be rolled once for the area. Range is 0 to 100. Eg. if a whole area of BlackForest rolls higher than the indicated chance, this spawn template will never be active in that forest. Another BlackForest will have another roll however, that may activate this template there. Chance is rolled based on world seed, area id and template index.");

    public ConfigurationEntry<string> ConditionLocation = new("", "Array of locations in which this spawn is enabled. If empty, allows all.\nEg. Runestone_Boars, FireHole");

    public ConfigurationEntry<string> ConditionAreaIds = new("", "Advanced feature. List of area id's in which the template is valid. Note: If ConditionSpawnChanceInArea is not 100 or disabled, it will still roll area chance.\nEg. 1, 123, 543");

    public ConfigurationEntry<string> ConditionBiome = new("", "Biomes in which entity can spawn. Leave empty for all.");

    public ConfigurationEntry<string> ConditionAllOfGlobalKeys = new("", "Global keys required to allow spawning. All listed keys must be present. Ignored if empty.");

    public ConfigurationEntry<string> ConditionAnyOfGlobalKeys = new("", "Global keys allowing spawning. One of the listed keys must be present. Ignored if empty.");

    public ConfigurationEntry<string> ConditionNoneOfGlobalKeys = new("", "Global keys disabling spawning. None of the listed keys must be present. Ignored if empty.");

    public ConfigurationEntry<string> ConditionEnvironment = new("", "List of environments required to allow spawning.\tEg. Misty, Thunderstorm. Leave empty to allow all.");

    public ConfigurationEntry<Daytime> ConditionDaytime = new(Daytime.All, "Toggles period in which spawning is active.");

    public ConfigurationEntry<float> ConditionAltitudeMin = new(-1000, "Minimum altitude (distance to water surface) to spawn in.");

    public ConfigurationEntry<float> ConditionAltitudeMax = new(10_000, "Maximum altitude (distance to water surface) to spawn in.");

    public ConfigurationEntry<bool> ConditionInForest = new(true, "Toggles spawning in forest.");

    public ConfigurationEntry<bool> ConditionOutsideForest = new(true, "Toggles spawning outside forest.");

    public ConfigurationEntry<float> ConditionOceanDepthMin = new(0, "Minimum ocean depth to spawn in. Ignored if min == max.");

    public ConfigurationEntry<float> ConditionOceanDepthMax = new(0, "Maximum ocean depth to spawn in. Ignored if min == max.");

    // Tilt - Gonna skip this one.

    #endregion
    #region PositionConditions

    #endregion
    #region Modifiers
    public ConfigurationEntry<string> SetFaction = new("", "Assign a specific faction to spawn. If empty uses default.");

    public ConfigurationEntry<bool> SetRelentless = new(false, "When true, forces mob AI to always be alerted.");

    // This is getting way overcomplicated and clumsy to support. Lets stop adding it in and mark it for deprecation.
    //public ConfigurationEntry<bool> SetTryDespawnOnConditionsInvalid = new(false, "When true, mob will try to run away and despawn when spawn conditions become invalid.\nEg. if spawning only during night, it will run away and despawn at night. Currently this only take into account conditions for daytime and environment.");

    public ConfigurationEntry<bool> SetTryDespawnOnAlert = new(false, "When true, mob will try to run away and despawn when alerted.");

    public ConfigurationEntry<bool> SetTamed = new(false, "When true, mob will be set to tamed status on spawn.");

    public ConfigurationEntry<bool> SetTamedCommandable = new(false, "Experimental. When true, will set mob as commandable when tamed. When false, whatever was default for the creature is used. Does not always seem to work for creatures not tameable in vanilla.");

    public ConfigurationEntry<bool> SetHuntPlayer = new(false, "Sets AI to hunt a player target.");

    #endregion
}

internal class DestructibleSpawnConfigCLLC : Config
{
    public const string ModName = "CreatureLevelAndLootControl";

    public ConfigurationEntry<int> ConditionWorldLevelMin = new(0, "Minimum CLLC world level for spawn to activate.");

    public ConfigurationEntry<int> ConditionWorldLevelMax = new(0, "Maximum CLLC world level for spawn to active. 0 means no max.");

    public ConfigurationEntry<string> SetInfusion = new("", "Assigns the specified infusion to creature spawned. Ignored if empty.");

    public ConfigurationEntry<string> SetExtraEffect = new("", "Assigns the specified effect to creature spawned. Ignored if empty.");

    public ConfigurationEntry<string> SetBossAffix = new("", "Assigns the specified boss affix to creature spawned. Only works for the default 5 bosses. Ignored if empty.");

    public ConfigurationEntry<bool> UseDefaultLevels = new(false, "Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control.");
}

internal class DestructibleSpawnConfigMobAI : Config
{
    public const string ModName = "MobAI";

    public ConfigurationEntry<string> SetAI = new("", "Name of MobAI to register for spawn. Eg. the defaults 'Fixer' and 'Worker'.");

    public ConfigurationFileEntry AIConfigFile = new("", "Configuration file to use for the SetAI. Eg. 'MyFixerConfig.json', can include path, but will always start searching from config folder. See MobAI documentation for file setup.");
}

internal class DestructibleSpawnConfigEpicLoot : Config
{
    public const string ModName = "EpicLoot";

    public ConfigurationEntry<string> ConditionNearbyPlayerCarryItemWithRarity = new("", "Checks if nearby players have any items of the listed rarities.\nEg. Magic, Legendary");

    public ConfigurationEntry<string> ConditionNearbyPlayerCarryLegendaryItem = new("", "Checks if nearby players have any of the listed epic loot legendary id's in inventory.\nEg. HeimdallLegs, RagnarLegs");
}