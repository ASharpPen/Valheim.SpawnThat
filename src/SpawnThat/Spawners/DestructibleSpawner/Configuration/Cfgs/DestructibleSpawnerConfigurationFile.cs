using SpawnThat.Core.Configuration;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration.Cfgs;

internal class DestructibleSpawnerConfigurationFile
{
}

internal class DestructibleSpawnerConfig
{
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
}

internal class DestructibleSpawnConfig
{
    public ConfigurationEntry<bool> Enabled = new(true, "Toggles this template. If disabled, this spawn entry will never be selected for spawning. Can be used to disable existing spawn entries.");

    public ConfigurationEntry<bool> TemplateEnabled = new(true, "Toggles this configuration on / off. If disabled, template will be ignored. Cannot be used to disable existing spawn entries.");

    public ConfigurationEntry<string> PrefabName = new("", "Prefab name of entity to spawn.");

    public ConfigurationEntry<string> 

}

internal class DestructibleSpawnConfigCLLC : Config
{
    public const string ModName = "CreatureLevelAndLootControl";

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