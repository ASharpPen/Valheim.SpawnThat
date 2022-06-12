using System.Collections.Generic;
using SpawnThat.Core.Toml;
using SpawnThat.Integrations.CLLC.Models;
using SpawnThat.Integrations.EpicLoot.Models;
using SpawnThat.Utilities.Enums;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Configuration.BepInEx;

internal class SpawnAreaSpawnerConfigurationFile
    : TomlConfigWithSubsections<SpawnAreaSpawnerConfig>
    , ITomlConfigFile
{
    protected override SpawnAreaSpawnerConfig InstantiateSubsection(string subsectionName)
    {
        return new SpawnAreaSpawnerConfig();
    }
}

internal class SpawnAreaSpawnerConfig
    : TomlConfigWithSubsections<SpawnAreaSpawnConfig>
{
    protected override SpawnAreaSpawnConfig InstantiateSubsection(string subsectionName)
    {
        return new SpawnAreaSpawnConfig();
    }

    #region Identifiers

    public TomlConfigEntry<List<string>> IdentifyByName = new("IdentifyByName", new(), "List of spawner prefab names to enable config for. Ignored if empty.");
    public TomlConfigEntry<List<Heightmap.Biome>> IdentifyByBiome = new("IdentifyByBiome", new(), "List of biomes to enable config in. Ignored if empty.");
    public TomlConfigEntry<List<string>> IdentifyByLocation = new("IdentifyByLocation", new(), "List of Locations to enable config in. Ignored if empty.");
    public TomlConfigEntry<List<string>> IdentifyByRoom = new("IdentifyByRoom", new(), "List of rooms (eg., dungeons, camp buildings) to enable config in. Ignored if empty.");

    #endregion

    #region Default Settings

    public TomlConfigEntry<float?> LevelUpChance = new("LevelUpChance", 15, "Chance to level up from MinLevel. Range 0 to 100.");

    public TomlConfigEntry<float?> SpawnInterval = new("SpawnInterval", 10, "Seconds between spawn checks.");

    public TomlConfigEntry<bool?> SetPatrol = new("SetPatrol", false, "Sets if spawn should patrol its spawn point.");

    public TomlConfigEntry<float?> ConditionPlayerWithinDistance = new("ConditionPlayerWithinDistance", 60, "Minimum distance to player for enabling spawn.");

    public TomlConfigEntry<int?> ConditionMaxCloseCreatures = new("ConditionMaxCloseCreatures", 3, "Sets maximum number of creatures within DistanceConsideredClose, for spawner to be active.");

    public TomlConfigEntry<int?> ConditionMaxCreatures = new("ConditionMaxCreatures", 100, "Sets maximum number of creatures currently loaded, for spawner to be active.");

    public TomlConfigEntry<float?> DistanceConsideredClose = new("DistanceConsideredClose", 20, "Distance within which another entity is counted as being close to spawner.");

    public TomlConfigEntry<float?> DistanceConsideredFar = new("DistanceConsideredFar", 1000, "Distance within which another entity is counted as being far to spawner.");

    public TomlConfigEntry<bool?> OnGroundOnly = new("OnGroundOnly", false, "Only spawn if spawn point is on the ground (ie., not in a dungeon) and open to the sky.");

    #endregion

    #region Custom Settings

    //public TomlConfigEntry<bool> Enabled = new("Enabled", true, "")

    public TomlConfigEntry<bool?> RemoveNotConfiguredSpawns = new("RemoveNotConfiguredSpawns", false, "Sets if spawns of spawner that were not configured should be removed.");

    #endregion
}

internal class SpawnAreaSpawnConfig : TomlConfigWithSubsections<TomlConfig>
{
    private SpawnAreaSpawnConfigMobAI _mobAIConfig = null;
    private SpawnAreaSpawnConfigCLLC _cllcConfig = null;
    private SpawnAreaSpawnConfigEpicLoot _epicLootConfig = null;

    public SpawnAreaSpawnConfigEpicLoot EpicLootConfig => _epicLootConfig ??= GetSubsection(SpawnAreaSpawnConfigEpicLoot.ModName) as SpawnAreaSpawnConfigEpicLoot;

    public SpawnAreaSpawnConfigCLLC CllcConfig => _cllcConfig ??= GetSubsection(SpawnAreaSpawnConfigCLLC.ModName) as SpawnAreaSpawnConfigCLLC;

    public SpawnAreaSpawnConfigMobAI MobAIConfig => _mobAIConfig ??= GetSubsection(SpawnAreaSpawnConfigMobAI.ModName) as SpawnAreaSpawnConfigMobAI;

    protected override TomlConfig InstantiateSubsection(string subsectionName)
    {
        TomlConfig newModConfig = null;

        if (subsectionName == SpawnAreaSpawnConfigCLLC.ModName.Trim().ToUpperInvariant())
        {
            newModConfig = _cllcConfig = new();
        }
        else if (subsectionName == SpawnAreaSpawnConfigMobAI.ModName.Trim().ToUpperInvariant())
        {
            newModConfig = _mobAIConfig = new();
        }
        else if (subsectionName == SpawnAreaSpawnConfigEpicLoot.ModName.Trim().ToUpperInvariant())
        {
            newModConfig = _epicLootConfig = new();
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

    public TomlConfigEntry<bool?> Enabled = new("Enabled", true, "Toggles this template. If disabled, this spawn entry will never be selected for spawning. Can be used to disable existing spawn entries.");

    public TomlConfigEntry<bool?> TemplateEnabled = new("TemplateEnabled", true, "Toggles this configuration on / off. If disabled, template will be ignored. Cannot be used to disable existing spawn entries.");

    public TomlConfigEntry<string> PrefabName = new("PrefabName", "", "Prefab name of entity to spawn.");

    public TomlConfigEntry<float?> SpawnWeight = new("SpawnWeight", 1, "Sets spawn weight. SpawnArea spawners choose their next spawn by a weighted random of all their possible spawns.\nIncreasing weight means an increased chance that this particular spawn will be selected for spawning.");

    public TomlConfigEntry<int?> LevelMin = new("LevelMin", 1, "Minimum level to spawn at.");

    public TomlConfigEntry<int?> LevelMax = new("LevelMax", 1, "Maximum level to spawn at.");

    #region Conditions
    public TomlConfigEntry<float?> ConditionDistanceToCenterMin = new("ConditionDistanceToCenterMin", 0, "Minimum distance to center for configuration to apply.");

    public TomlConfigEntry<float?> ConditionDistanceToCenterMax = new("ConditionDistanceToCenterMax", 0, "Maximum distance to center for configuration to apply. 0 means limitless.");

    public TomlConfigEntry<int?> ConditionWorldAgeDaysMin = new("ConditionWorldAgeDaysMin", 0, "Minimum world age in in-game days for this configuration to apply.");

    public TomlConfigEntry<int?> ConditionWorldAgeDaysMax = new("ConditionWorldAgeDaysMax", 0, "Maximum world age in in-game days for this configuration to apply. 0 means no max.");

    public TomlConfigEntry<float?> DistanceToTriggerPlayerConditions = new("DistanceToTriggerPlayerConditions", 100, "Distance of player to spawner, for player to be included in player based checks such as ConditionNearbyPlayersCarryValue.");

    public TomlConfigEntry<int?> ConditionNearbyPlayersCarryValue = new("ConditionNearbyPlayersCarryValue", 0, "Checks if nearby players have a combined value in inventory above this condition.\nEg. If set to 100, entry will only activate if nearby players have more than 100 worth of values combined.");

    public TomlConfigEntry<List<string>> ConditionNearbyPlayerCarriesItem = new("ConditionNearbyPlayerCarriesItem", new(), "Checks if nearby players have any of the listed item prefab names in inventory.\nEg. IronScrap, DragonEgg");

    public TomlConfigEntry<float?> ConditionNearbyPlayersNoiseThreshold = new("ConditionNearbyPlayersNoiseThreshold", 0, "Checks if any nearby players have accumulated noise at or above the threshold.");

    public TomlConfigEntry<List<string>> ConditionNearbyPlayersStatus = new("ConditionNearbyPlayersStatus", new(), "Checks if any nearbly players have any of the listed status effects.\nEg. Wet, Burning");

    public TomlConfigEntry<float?> ConditionAreaSpawnChance = new("ConditionAreaSpawnChance", 100, "Chance for spawn to spawn at all in the area. The chance will be rolled once for the area. Range is 0 to 100. Eg. if a whole area of BlackForest rolls higher than the indicated chance, this spawn template will never be active in that forest. Another BlackForest will have another roll however, that may activate this template there. Chance is rolled based on world seed, area id and template index.");

    public TomlConfigEntry<List<string>> ConditionLocation = new("ConditionLocation", new(), "Array of locations in which this spawn is enabled. If empty, allows all.\nEg. Runestone_Boars, FireHole");

    public TomlConfigEntry<List<int>> ConditionAreaIds = new("ConditionAreaIds", new(), "Advanced feature. List of area id's in which the template is valid. Note: If ConditionSpawnChanceInArea is not 100 or disabled, it will still roll area chance.\nEg. 1, 123, 543");

    public TomlConfigEntry<List<Heightmap.Biome>> ConditionBiome = new("ConditionBiome", new(), "Biomes in which entity can spawn. Leave empty for all.");

    public TomlConfigEntry<List<string>> ConditionAllOfGlobalKeys = new("ConditionAllOfGlobalKeys", new(), "Global keys required to allow spawning. All listed keys must be present. Ignored if empty.");

    public TomlConfigEntry<List<string>> ConditionAnyOfGlobalKeys = new("ConditionAnyOfGlobalKeys", new(), "Global keys allowing spawning. One of the listed keys must be present. Ignored if empty.");

    public TomlConfigEntry<List<string>> ConditionNoneOfGlobalKeys = new("ConditionNoneOfGlobalKeys", new(), "Global keys disabling spawning. None of the listed keys must be present. Ignored if empty.");

    public TomlConfigEntry<List<string>> ConditionEnvironment = new("ConditionEnvironment", new(), "List of environments required to allow spawning.\tEg. Misty, Thunderstorm. Leave empty to allow all.");

    public TomlConfigEntry<List<Daytime>> ConditionDaytime = new("ConditionDaytime", new(), "Toggles period in which spawning is active.");

    public TomlConfigEntry<float?> ConditionAltitudeMin = new("ConditionAltitudeMin", -1000, "Minimum altitude (distance to water surface) to spawn in.");

    public TomlConfigEntry<float?> ConditionAltitudeMax = new("ConditionAltitudeMax", 10_000, "Maximum altitude (distance to water surface) to spawn in.");

    public TomlConfigEntry<ForestState?> ConditionForestState = new("ConditionForestState", ForestState.Both, "Toggles spawning when inside the specified state of forestation. Note that the forestation is based on world generation.");

    public TomlConfigEntry<float?> ConditionOceanDepthMin = new("ConditionOceanDepthMin", 0, "Minimum ocean depth to spawn in. Ignored if min == max.");

    public TomlConfigEntry<float?> ConditionOceanDepthMax = new("ConditionOceanDepthMax", 0, "Maximum ocean depth to spawn in. Ignored if min == max.");

    // Tilt - Gonna skip this one.

    #endregion
    #region PositionConditions

    #endregion
    #region Modifiers
    public TomlConfigEntry<Character.Faction?> SetFaction = new("SetFaction", Character.Faction.Boss, "Assign a specific faction to spawn. If empty uses default.");

    public TomlConfigEntry<bool?> SetRelentless = new("SetRelentless", false, "When true, forces mob AI to always be alerted.");

    // This is getting way overcomplicated and clumsy to support. Lets stop adding it in and mark it for deprecation.
    //public TomlConfigEntry<bool> SetTryDespawnOnConditionsInvalid = new("SetTryDespawnOnConditionsInvalid", false, "When true, mob will try to run away and despawn when spawn conditions become invalid.\nEg. if spawning only during night, it will run away and despawn at night. Currently this only take into account conditions for daytime and environment.");

    public TomlConfigEntry<bool?> SetTryDespawnOnAlert = new("SetTryDespawnOnAlert", false, "When true, mob will try to run away and despawn when alerted.");

    public TomlConfigEntry<bool?> SetTamed = new("SetTamed", false, "When true, mob will be set to tamed status on spawn.");

    public TomlConfigEntry<bool?> SetTamedCommandable = new("SetTamedCommandable", false, "Experimental. When true, will set mob as commandable when tamed. When false, whatever was default for the creature is used. Does not always seem to work for creatures not tameable in vanilla.");

    public TomlConfigEntry<bool?> SetHuntPlayer = new("SetHuntPlayer", false, "Sets AI to hunt a player target.");

    #endregion
}

internal class SpawnAreaSpawnConfigCLLC : TomlConfig
{
    public const string ModName = "CreatureLevelAndLootControl";

    public TomlConfigEntry<int?> ConditionWorldLevelMin = new("ConditionWorldLevelMin", 0, "Minimum CLLC world level for spawn to activate.");

    public TomlConfigEntry<int?> ConditionWorldLevelMax = new("ConditionWorldLevelMax", 0, "Maximum CLLC world level for spawn to active. 0 means no max.");

    public TomlConfigEntry<CllcCreatureInfusion?> SetInfusion = new("SetInfusion", CllcCreatureInfusion.None, "Assigns the specified infusion to creature spawned. Ignored if empty.");

    public TomlConfigEntry<CllcCreatureExtraEffect?> SetExtraEffect = new("SetExtraEffect", CllcCreatureExtraEffect.None, "Assigns the specified effect to creature spawned. Ignored if empty.");

    public TomlConfigEntry<CllcBossAffix?> SetBossAffix = new("SetBossAffix", CllcBossAffix.None, "Assigns the specified boss affix to creature spawned. Only works for the default 5 bosses. Ignored if empty.");

    public TomlConfigEntry<bool?> UseDefaultLevels = new("UseDefaultLevels", false, "Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control.");
}

internal class SpawnAreaSpawnConfigMobAI : TomlConfig
{
    public const string ModName = "MobAI";

    public TomlConfigEntry<string> SetAI = new("SetAI", "", "Name of MobAI to register for spawn. Eg. the defaults 'Fixer' and 'Worker'.");

    public TomlConfigEntry<string> AIConfigFile = new("AIConfigFile", "", "Configuration file to use for the SetAI. Eg. 'MyFixerConfig.json', can include path, but will always start searching from config folder. See MobAI documentation for file setup.");
}

internal class SpawnAreaSpawnConfigEpicLoot : TomlConfig
{
    public const string ModName = "EpicLoot";

    public TomlConfigEntry<List<EpicLootRarity>> ConditionNearbyPlayerCarryItemWithRarity = new("ConditionNearbyPlayerCarryItemWithRarity", new(), "Checks if nearby players have any items of the listed rarities.\nEg. Magic, Legendary");

    public TomlConfigEntry<List<string>> ConditionNearbyPlayerCarryLegendaryItem = new("ConditionNearbyPlayerCarryLegendaryItem", new(), "Checks if nearby players have any of the listed epic loot legendary id's in inventory.\nEg. HeimdallLegs, RagnarLegs");
}