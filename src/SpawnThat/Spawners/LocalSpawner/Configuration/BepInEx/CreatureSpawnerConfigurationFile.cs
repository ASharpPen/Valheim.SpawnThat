using System;
using SpawnThat.Core.Configuration;
using SpawnThat.Core.Toml;

namespace SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;

internal class CreatureSpawnerConfigurationFile : TomlConfigWithSubsections<CreatureLocationConfig>, ITomlConfigFile
{
    protected override CreatureLocationConfig InstantiateSubsection(string subsectionName)
    {
        return new CreatureLocationConfig();
    }
}

internal class CreatureLocationConfig : TomlConfigWithSubsections<CreatureSpawnerConfig>
{
    protected override CreatureSpawnerConfig InstantiateSubsection(string subsectionName)
    {
        return new CreatureSpawnerConfig();
    }
}

internal class CreatureSpawnerConfig : TomlConfigWithSubsections<TomlConfig>
{
    protected override TomlConfig InstantiateSubsection(string subsectionName)
    {
        TomlConfig newModConfig = null;

        if (subsectionName == CreatureSpawnerConfigCLLC.ModName.Trim().ToUpperInvariant())
        {
            newModConfig = new CreatureSpawnerConfigCLLC();
        }
        else if (subsectionName == CreatureSpawnerConfigMobAI.ModName.Trim().ToUpperInvariant())
        {
            newModConfig = new CreatureSpawnerConfigMobAI();
        }

        return newModConfig;
    }

    public TomlConfigEntry<string> PrefabName = new("PrefabName", "", "PrefabName of entity to spawn.");

    public TomlConfigEntry<bool> Enabled = new("Enabled", true, "Enable/disable this spawner.");

    public TomlConfigEntry<bool> TemplateEnabled = new("TemplateEnabled", true, "Enable/disable this configuration. Does not disable the spawner itself.");

    #region New options

    //TODO: Need to mod in additional checks I think.
    //public ConfigurationEntry<string> Environments = new ConfigurationEntry<string>("", "Array of Environments required for spawning.");

    //TODO: Need to mod in additional checks I think.
    //public ConfigurationEntry<string> RequiredGlobalKeys = new ConfigurationEntry<string>();

    #endregion

    #region Default options

    public TomlConfigEntry<bool> SpawnAtDay = new("SpawnAtDay", true, "Enable spawning during day.");
    public TomlConfigEntry<bool> SpawnAtNight = new("SpawnAtNight", true, "Enable spawning during night.");
    public TomlConfigEntry<int> LevelMin = new("LevelMin", 1, "Minimum level of spawn.");
    public TomlConfigEntry<int> LevelMax = new("LevelMax", 1, "Maximum level of spawn.");
    public TomlConfigEntry<float> LevelUpChance = new("LevelUpChance", 10, "Chance to level up, starting at minimum level and rolling again for each level gained. Range is 0 to 100.");
    public TomlConfigEntry<float> RespawnTime = new("RespawnTime", 20, "Minutes between checks for respawn. Only one mob can be spawned at time per spawner.");
    public TomlConfigEntry<float> TriggerDistance = new("TriggerDistance", 60, "Distance to trigger spawning.");
    public TomlConfigEntry<float> TriggerNoise = new("TriggerNoise", 0, "If not 0, adds a minimum noise required for spawning, on top of distance requirement.");
    public TomlConfigEntry<bool> SpawnInPlayerBase = new("SpawnInPlayerBase", false, "Allow spawning inside player base boundaries.");
    public TomlConfigEntry<bool> SetPatrolPoint = new("SetPatrolPoint", false, "Sets position of spawn as patrol point.");
    public TomlConfigEntry<string> SetFaction = new("SetFaction", "", "Assign a specific faction to spawn. If empty uses default.");
    public TomlConfigEntry<bool> SetTamed = new("SetTamed", false, "When true, mob will be set to tamed status on spawn.");
    public TomlConfigEntry<bool> SetTamedCommandable = new("SetTamedCommandable", false, "Experimental. When true, will set mob as commandable when tamed. When false, whatever was default for the creature is used. Does not always seem to work for creatures not tameable in vanilla.");

    //Doesn't seem to have a purpose for now, lets leave it out.
    //public ConfigurationEntry<bool> RequireSpawnArea = new ConfigurationEntry<bool>(false, "Needs a clear area maybe? Can't find a use case for this one, may be pointless for now.");

    //TODO: Lets get this mapped up in the future, it sounds cool!
    //public ConfigurationEntry<string> EffectList = new ConfigurationEntry<string>("", "");

    #endregion
}

internal class CreatureSpawnerConfigCLLC : TomlConfig
{
    public const string ModName = "CreatureLevelAndLootControl";

    public TomlConfigEntry<string> SetInfusion = new("SetInfusion", "", "Assigns the specified infusion to creature spawned. Ignored if empty.");

    public TomlConfigEntry<string> SetExtraEffect = new("SetExtraEffect", "", "Assigns the specified effect to creature spawned. Ignored if empty.");

    public TomlConfigEntry<string> SetBossAffix = new("SetBossAffix", "", "Assigns the specified boss affix to creature spawned. Only works for the default 5 bosses. Ignored if empty.");

    public TomlConfigEntry<bool> UseDefaultLevels = new("UseDefaultLevels", false, "Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control.");
}

internal class CreatureSpawnerConfigMobAI : TomlConfig
{
    public const string ModName = "MobAI";

    public TomlConfigEntry<string> SetAI = new("SetAI", "", "Name of MobAI to register for spawn. Eg. the defaults 'Fixer' and 'Worker'.");

    public TomlConfigFileEntry AIConfigFile = new("AIConfigFile", "", "Configuration file to use for the SetAI. Eg. 'MyFixerConfig.json', can include path, but will always start searching from config folder. See MobAI documentation for file setup.");
}
