using System;
using Valheim.SpawnThat.Core.Configuration;

namespace Valheim.SpawnThat.Configuration.ConfigTypes
{
    [Serializable]
    public class CreatureSpawnerConfigurationFile : ConfigWithSubsections<CreatureLocationConfig>, IConfigFile
    {
        protected override CreatureLocationConfig InstantiateSubsection(string subsectionName)
        {
            return new CreatureLocationConfig();
        }
    }

    [Serializable]
    public class CreatureLocationConfig : ConfigWithSubsections<CreatureSpawnerConfig>
    {
        protected override CreatureSpawnerConfig InstantiateSubsection(string subsectionName)
        {
            return new CreatureSpawnerConfig();
        }
    }

    [Serializable]
    public class CreatureSpawnerConfig : ConfigWithSubsections<Config>
    {
        protected override Config InstantiateSubsection(string subsectionName)
        {
            Config newModConfig = null;

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

        public ConfigurationEntry<string> PrefabName = new ConfigurationEntry<string>("", "PrefabName of entity to spawn.");

        public ConfigurationEntry<bool> Enabled = new ConfigurationEntry<bool>(true, "Enable/disable this configuration.");

        #region New options

        //TODO: Need to mod in additional checks I think.
        //public ConfigurationEntry<string> Environments = new ConfigurationEntry<string>("", "Array of Environments required for spawning.");

        //TODO: Need to mod in additional checks I think.
        //public ConfigurationEntry<string> RequiredGlobalKeys = new ConfigurationEntry<string>();

        #endregion

        #region Default options

        public ConfigurationEntry<bool> SpawnAtDay = new ConfigurationEntry<bool>(true, "Enable spawning during day.");
        public ConfigurationEntry<bool> SpawnAtNight = new ConfigurationEntry<bool>(true, "Enable spawning during night.");
        public ConfigurationEntry<int> LevelMin = new ConfigurationEntry<int>(1, "Minimum level of spawn.");
        public ConfigurationEntry<int> LevelMax = new ConfigurationEntry<int>(1, "Maximum level of spawn.");
        public ConfigurationEntry<float> LevelUpChance = new ConfigurationEntry<float>(10, "Chance to level up, starting at minimum level and rolling again for each level gained. Range is 0 to 100.");
        public ConfigurationEntry<float> RespawnTime = new ConfigurationEntry<float>(20, "Minutes between checks for respawn. Only one mob can be spawned at time per spawner.");
        public ConfigurationEntry<float> TriggerDistance = new ConfigurationEntry<float>(60, "Distance to trigger spawning.");
        public ConfigurationEntry<float> TriggerNoise = new ConfigurationEntry<float>(0, "If not 0, adds a minimum noise required for spawning, on top of distance requirement.");
        public ConfigurationEntry<bool> SpawnInPlayerBase = new ConfigurationEntry<bool>(false, "Allow spawning inside player base boundaries.");
        public ConfigurationEntry<bool> SetPatrolPoint = new ConfigurationEntry<bool>(false, "Sets position of spawn as patrol point.");
        public ConfigurationEntry<string> SetFaction = new ConfigurationEntry<string>("", "Assign a specific faction to spawn. If empty uses default.");
        public ConfigurationEntry<bool> SetTamed = new ConfigurationEntry<bool>(false, "When true, mob will be set to tamed status on spawn.");
        public ConfigurationEntry<bool> SetTamedCommandable = new ConfigurationEntry<bool>(false, "Experimental. When true, will set mob as commandable when tamed. When false, whatever was default for the creature is used. Does not always seem to work for creatures not tameable in vanilla.");

        //Doesn't seem to have a purpose for now, lets leave it out.
        //public ConfigurationEntry<bool> RequireSpawnArea = new ConfigurationEntry<bool>(false, "Needs a clear area maybe? Can't find a use case for this one, may be pointless for now.");

        //TODO: Lets get this mapped up in the future, it sounds cool!
        //public ConfigurationEntry<string> EffectList = new ConfigurationEntry<string>("", "");

        #endregion
    }

    [Serializable]
    public class CreatureSpawnerConfigCLLC : Config
    {
        public const string ModName = "CreatureLevelAndLootControl";

        public ConfigurationEntry<string> SetInfusion = new ConfigurationEntry<string>("", "Assigns the specified infusion to creature spawned. Ignored if empty.");

        public ConfigurationEntry<string> SetExtraEffect = new ConfigurationEntry<string>("", "Assigns the specified effect to creature spawned. Ignored if empty.");

        public ConfigurationEntry<string> SetBossAffix = new ConfigurationEntry<string>("", "Assigns the specified boss affix to creature spawned. Only works for the default 5 bosses. Ignored if empty.");

        public ConfigurationEntry<bool> UseDefaultLevels = new ConfigurationEntry<bool>(false, "Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control.");
    }

    [Serializable]
    public class CreatureSpawnerConfigMobAI : Config
    {
        public const string ModName = "MobAI";

        public ConfigurationEntry<string> SetAI = new ConfigurationEntry<string>("", "Name of MobAI to register for spawn. Eg. the defaults 'Fixer' and 'Worker'.");

        public ConfigurationFileEntry AIConfigFile = new ConfigurationFileEntry("", "Configuration file to use for the SetAI. Eg. 'MyFixerConfig.json', can include path, but will always start searching from config folder. See MobAI documentation for file setup.");
    }
}
