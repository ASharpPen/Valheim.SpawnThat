using System.Collections.Generic;
using SpawnThat.Core.Toml;
using SpawnThat.Integrations.CLLC.Models;
using SpawnThat.Integrations.EpicLoot.Models;

namespace SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

internal class SpawnSystemConfigurationFile : TomlConfigWithSubsections<SpawnSystemConfiguration>, ITomlConfigFile
{
    protected override SpawnSystemConfiguration InstantiateSubsection(string subsectionName)
    {
        return new SpawnSystemConfiguration();
    }
}

internal class SpawnSystemConfiguration : TomlConfigWithSubsections<SpawnConfiguration>
{
    protected override SpawnConfiguration InstantiateSubsection(string subsectionName)
    {
        return new SpawnConfiguration();
    }
}

internal class SpawnConfiguration : TomlConfigWithSubsections<TomlConfig>
{
    private SpawnSystemConfigCLLC _cllcConfig;
    private SpawnSystemConfigMobAI _mobAIConfig;
    private SpawnSystemConfigEpicLoot _epicLootConfig;

    public SpawnSystemConfigCLLC CllcConfig => _cllcConfig ??= GetSubsection(SpawnSystemConfigCLLC.ModName) as SpawnSystemConfigCLLC;
    public SpawnSystemConfigMobAI MobAIConfig => _mobAIConfig ??= GetSubsection(SpawnSystemConfigMobAI.ModName) as SpawnSystemConfigMobAI;
    public SpawnSystemConfigEpicLoot EpicLootConfig => _epicLootConfig ??= GetSubsection(SpawnSystemConfigEpicLoot.ModName) as SpawnSystemConfigEpicLoot;

    protected override TomlConfig InstantiateSubsection(string subsectionName)
    {
        TomlConfig newModConfig = null;

        if (subsectionName == SpawnSystemConfigCLLC.ModName.Trim().ToUpperInvariant())
        {
            newModConfig = _cllcConfig = new SpawnSystemConfigCLLC();
        }
        else if (subsectionName == SpawnSystemConfigMobAI.ModName.Trim().ToUpperInvariant())
        {
            newModConfig = _mobAIConfig = new SpawnSystemConfigMobAI();
        }
        else if (subsectionName == SpawnSystemConfigEpicLoot.ModName.Trim().ToUpperInvariant())
        {
            newModConfig = _epicLootConfig = new SpawnSystemConfigEpicLoot();
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

    public TomlConfigEntry<string> TemplateId = new("TemplateId", "", "Technical setting intended for cross-mod identification of mobs spawned by this template. Sets a custom identifier which will be assigned to the spawned mobs ZDO as 'ZDO.Set(\"spawn_template_id\", TemplateIdentifier)'.");

    public TomlConfigEntry<string> Name = new("Name", "My spawner", "Just a field for naming the configuration entry.");

    public TomlConfigEntry<bool?> Enabled = new("Enabled", true, "Enable/disable this spawner entry.");

    public TomlConfigEntry<bool?> TemplateEnabled = new("TemplateEnabled", true, "Enable/disable this configuration. This does not disable existing entries, just this configuration itself.");

    public TomlConfigEntry<List<Heightmap.Biome>> Biomes = new("Biomes", new(), "Biomes in which entity can spawn. Leave empty for all.");

    //public ConfigurationEntry<bool> DriveInward = new ConfigurationEntry<bool>(false, "Mobs always spawn towards the world edge from player.");

    //Bound to the spawner itself. Need to transpile in a change for this to work.
    //public ConfigurationEntry<float> LevelUpChance = new ConfigurationEntry<float>(10, "Chance to increase level above min. This is run multiple times. 100 is 100%.\nEg. if Chance is 10, LevelMin is 1 and LevelMax is 3, the game will have a 10% to become level 2. The game will then run an additional 10% check for increasing to level 3.");

    public TomlConfigEntry<float?> ConditionDistanceToCenterMin = new("ConditionDistanceToCenterMin", 0, "Minimum distance to center for configuration to apply.");

    public TomlConfigEntry<float?> ConditionDistanceToCenterMax = new("ConditionDistanceToCenterMax", 0, "Maximum distance to center for configuration to apply. 0 means limitless.");

    public TomlConfigEntry<float?> ConditionWorldAgeDaysMin = new("ConditionWorldAgeDaysMin", 0, "Minimum world age in in-game days for this configuration to apply.");

    public TomlConfigEntry<float?> ConditionWorldAgeDaysMax = new("ConditionWorldAgeDaysMax", 0, "Maximum world age in in-game days for this configuration to apply. 0 means no max.");

    public TomlConfigEntry<float?> DistanceToTriggerPlayerConditions = new("DistanceToTriggerPlayerConditions", 100, "Distance of player to spawner, for player to be included in player based checks such as ConditionNearbyPlayersCarryValue.");

    public TomlConfigEntry<int?> ConditionNearbyPlayersCarryValue = new("ConditionNearbyPlayersCarryValue", 0, "Checks if nearby players have a combined value in inventory above this condition.\nEg. If set to 100, entry will only activate if nearby players have more than 100 worth of values combined.");

    public TomlConfigEntry<List<string>> ConditionNearbyPlayerCarriesItem = new("ConditionNearbyPlayerCarriesItem", new(), "Checks if nearby players have any of the listed item prefab names in inventory.\nEg. IronScrap, DragonEgg");

    public TomlConfigEntry<float?> ConditionNearbyPlayersNoiseThreshold = new("ConditionNearbyPlayersNoiseThreshold", 0, "Checks if any nearby players have accumulated noise at or above the threshold.");

    public TomlConfigEntry<List<string>> ConditionNearbyPlayersStatus = new("ConditionNearbyPlayersStatus", new(), "Checks if any nearbly players have any of the listed status effects.\nEg. Wet, Burning");

    public TomlConfigEntry<List<string>> RequiredNotGlobalKey = new("RequiredNotGlobalKey", new(), "Array of global keys which disable the spawning of this entity if any are detected.\nEg. defeated_bonemass,KilledTroll");

    public TomlConfigEntry<Character.Faction?> SetFaction = new("SetFaction", Character.Faction.Boss, "Assign a specific faction to spawn. If empty uses default.");

    public TomlConfigEntry<bool?> SetRelentless = new("SetRelentless", false, "When true, forces mob AI to always be alerted.");

    public TomlConfigEntry<bool?> SetTryDespawnOnConditionsInvalid = new("SetTryDespawnOnConditionsInvalid", false, "When true, mob will try to run away and despawn when spawn conditions become invalid.\nEg. if spawning only during night, it will run away and despawn at night. Currently this only take into account conditions for daytime and environment.");

    public TomlConfigEntry<bool?> SetTryDespawnOnAlert = new("SetTryDespawnOnAlert", false, "When true, mob will try to run away and despawn when alerted.");

    public TomlConfigEntry<bool?> SetTamed = new("SetTamed", false, "When true, mob will be set to tamed status on spawn.");

    public TomlConfigEntry<bool?> SetTamedCommandable = new("SetTamedCommandable", false, "Experimental. When true, will set mob as commandable when tamed. When false, whatever was default for the creature is used. Does not always seem to work for creatures not tameable in vanilla.");

    public TomlConfigEntry<List<string>> ConditionLocation = new("ConditionLocation", new(), "Array of locations in which this spawn is enabled. If empty, allows all.\nEg. Runestone_Boars, FireHole");

    public TomlConfigEntry<float?> ConditionAreaSpawnChance = new("ConditionAreaSpawnChance", 100, "Chance for spawn to spawn at all in the area. The chance will be rolled once for the area. Range is 0 to 100. Eg. if a whole area of BlackForest rolls higher than the indicated chance, this spawn template will never be active in that forest. Another BlackForest will have another roll however, that may activate this template there. Chance is rolled based on world seed, area id and template index.");

    public TomlConfigEntry<List<int>> ConditionAreaIds = new("ConditionAreaIds", new(), "Advanced feature. List of area id's in which the template is valid. Note: If ConditionSpawnChanceInArea is not 100 or disabled, it will still roll area chance.\nEg. 1, 123, 543");

    #region Default Configuration Options

    public TomlConfigEntry<string> PrefabName = new("PrefabName", "Deer", "Prefab name of the entity to spawn.");

    public TomlConfigEntry<bool?> HuntPlayer = new("HuntPlayer", false, "Sets AI to hunt a player target.");

    public TomlConfigEntry<int?> MaxSpawned = new("MaxSpawned", 1, "Maximum entities of type spawned in area.");

    public TomlConfigEntry<float?> SpawnInterval = new("SpawnInterval", 90, "Seconds between spawn checks.");

    public TomlConfigEntry<float?> SpawnChance = new("SpawnChance", 100, "Chance to spawn per check. Range 0 to 100.");

    public TomlConfigEntry<int?> LevelMin = new("LevelMin", 1, "Minimum level to spawn.");

    public TomlConfigEntry<int?> LevelMax = new("LevelMax", 1, "Maximum level to spawn.");

    public TomlConfigEntry<float?> LevelUpMinCenterDistance = new("LevelUpMinCenterDistance", 0, "Minimum distance from world center, to allow higher than min level.");

    public TomlConfigEntry<float?> SpawnDistance = new("SpawnDistance", 0, "Minimum distance to another entity.");

    public TomlConfigEntry<float?> SpawnRadiusMin = new("SpawnRadiusMin", 0, "Minimum spawn radius.");

    public TomlConfigEntry<float?> SpawnRadiusMax = new("SpawnRadiusMax", 0, "Maximum spawn radius.");

    public TomlConfigEntry<string> RequiredGlobalKey = new("RequiredGlobalKey", "", "Required global key to spawn.\nEg. defeated_bonemass");

    public TomlConfigEntry<List<string>> RequiredEnvironments = new("RequiredEnvironments", new(), "Array (separate by comma) of environments required to spawn in.\tEg. Misty, Thunderstorm. Leave empty to allow all.");

    public TomlConfigEntry<int?> GroupSizeMin = new("GroupSizeMin", 1, "Minimum count to spawn at per check.");

    public TomlConfigEntry<int?> GroupSizeMax = new("GroupSizeMax", 1, "Maximum count to spawn at per check.");

    public TomlConfigEntry<float?> GroupRadius = new("GroupRadius", 3, "Size of circle to spawn group inside.");

    public TomlConfigEntry<float?> GroundOffset = new("GroundOffset", 0.5f, "Offset to ground to spawn at.");

    public TomlConfigEntry<bool?> SpawnDuringDay = new("SpawnDuringDay", true, "Toggles spawning at day.");

    public TomlConfigEntry<bool?> SpawnDuringNight = new("SpawnDuringNight", true, "Toggles spawning at night.");

    public TomlConfigEntry<float?> ConditionAltitudeMin = new("ConditionAltitudeMin", -1000, "Minimum altitude (distance to water surface) to spawn in.");

    public TomlConfigEntry<float?> ConditionAltitudeMax = new("ConditionAltitudeMax", 1000, "Maximum altitude (distance to water surface) to spawn in.");

    public TomlConfigEntry<float?> ConditionTiltMin = new("ConditionTiltMin", 0, "Minium tilt of terrain to spawn in.");

    public TomlConfigEntry<float?> ConditionTiltMax = new("ConditionTiltMax", 35, "Maximum tilt of terrain to spawn in.");

    public TomlConfigEntry<bool?> SpawnInForest = new("SpawnInForest", true, "Toggles spawning in forest.");

    public TomlConfigEntry<bool?> SpawnOutsideForest = new("SpawnOutsideForest", true, "Toggles spawning outside of forest.");

    public TomlConfigEntry<float?> OceanDepthMin = new("OceanDepthMin", 0, "Minimum ocean depth to spawn in. Ignored if min == max.");

    public TomlConfigEntry<float?> OceanDepthMax = new("OceanDepthMax", 0, "Maximum ocean depth to spawn in. Ignored if min == max.");

    #endregion
}

internal class SpawnSystemConfigCLLC : TomlConfig
{
    public const string ModName = "CreatureLevelAndLootControl";

    public TomlConfigEntry<int?> ConditionWorldLevelMin = new("ConditionWorldLevelMin", -1, "Minimum CLLC world level for spawn to activate. Negative value disables this condition.");

    public TomlConfigEntry<int?> ConditionWorldLevelMax = new("ConditionWorldLevelMax", -1, "Maximum CLLC world level for spawn to active. Negative value disables this condition.");

    public TomlConfigEntry<CllcCreatureInfusion?> SetInfusion = new("SetInfusion", CllcCreatureInfusion.None, "Assigns the specified infusion to creature spawned. Ignored if empty.");

    public TomlConfigEntry<CllcCreatureExtraEffect?> SetExtraEffect = new("SetExtraEffect", CllcCreatureExtraEffect.None, "Assigns the specified effect to creature spawned. Ignored if empty.");

    public TomlConfigEntry<CllcBossAffix?> SetBossAffix = new("SetBossAffix", CllcBossAffix.None, "Assigns the specified boss affix to creature spawned. Only works for the default 5 bosses. Ignored if empty.");

    public TomlConfigEntry<bool?> UseDefaultLevels = new("UseDefaultLevels", false, "Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control.");
}

internal class SpawnSystemConfigMobAI : TomlConfig
{
    public const string ModName = "MobAI";

    public TomlConfigEntry<string> SetAI = new("SetAI", "", "Name of MobAI to register for spawn. Eg. the defaults 'Fixer' and 'Worker'.");

    public TomlConfigEntry<string> AIConfigFile = new("AIConfigFile", "", "Configuration file to use for the SetAI. Eg. 'MyFixerConfig.json', can include path, but will always start searching from config folder. See MobAI documentation for file setup.");
}

internal class SpawnSystemConfigEpicLoot : TomlConfig
{
    public const string ModName = "EpicLoot";

    public TomlConfigEntry<List<EpicLootRarity>> ConditionNearbyPlayerCarryItemWithRarity = new("ConditionNearbyPlayerCarryItemWithRarity", new(), "Checks if nearby players have any items of the listed rarities.\nEg. Magic, Legendary");

    public TomlConfigEntry<List<string>> ConditionNearbyPlayerCarryLegendaryItem = new("ConditionNearbyPlayerCarryLegendaryItem", new(), "Checks if nearby players have any of the listed epic loot legendary id's in inventory.\nEg. HeimdallLegs, RagnarLegs");
}
