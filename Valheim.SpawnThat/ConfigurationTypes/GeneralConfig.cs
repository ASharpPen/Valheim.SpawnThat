using BepInEx.Configuration;
using System;
using Valheim.SpawnThat.ConfigurationCore;

namespace Valheim.SpawnThat.ConfigurationTypes
{
    [Serializable]
    public class GeneralConfig
    {
        [NonSerialized]
        private ConfigFile Config;

        #region Debug

        public ConfigurationEntry<bool> DebugLoggingOn = new ConfigurationEntry<bool>(false, "Enable debug logging.");

        public ConfigurationEntry<bool> TraceLoggingOn = new ConfigurationEntry<bool>(false, "Enables in-depth logging. Note, this might generate a LOT of log entries.");

        #endregion

        #region General

        public ConfigurationEntry<bool> StopTouchingMyConfigs = new ConfigurationEntry<bool>(false, "Disables automatic updating and saving of drop table configurations.\nThis means no helpers will be added, but.. allows you to keep things compact.");

        #endregion

        #region WorldSpawner

        public ConfigurationEntry<bool> ClearAllExisting = new ConfigurationEntry<bool>(false, "If true, removes all existing world spawner templates.");

        public ConfigurationEntry<bool> AlwaysAppend = new ConfigurationEntry<bool>(false, "If true, will never override existing spawners, but add all custom configurations to the list.");

        public ConfigurationEntry<bool> WriteSpawnTablesToFileBeforeChanges = new ConfigurationEntry<bool>(false, "Dumps world spawner templates to a file, before applying custom changes.");

        public ConfigurationEntry<bool> WriteSpawnTablesToFileAfterChanges = new ConfigurationEntry<bool>(false, "Dumps world spawner templates to a file after applying configuration changes.");

        #endregion

        #region LocalSpawner

        public ConfigurationEntry<bool> WriteCreatureSpawnersToFile = new ConfigurationEntry<bool>(false, "Dumps local spawners to a file before applying configuration changes.");

        #endregion

        public void Load(ConfigFile configFile)
        {
            Config = configFile;

            DebugLoggingOn.Bind(Config, "Debug", "DebugLoggingOn");
            TraceLoggingOn.Bind(Config, "Debug", "TraceLoggingOn");

            WriteCreatureSpawnersToFile.Bind(Config, "LocalSpawner", "WriteSpawnTablesToFileBeforeChanges");

            ClearAllExisting.Bind(Config, "WorldSpawner", "ClearAllExisting");
            AlwaysAppend.Bind(Config, "WorldSpawner", "AlwaysAppend");
            WriteSpawnTablesToFileBeforeChanges.Bind(Config, "WorldSpawner", "WriteSpawnTablesToFileBeforeChanges");
            WriteSpawnTablesToFileAfterChanges.Bind(Config, "WorldSpawner", "WriteSpawnTablesToFileAfterChanges");

            StopTouchingMyConfigs.Bind(Config, "General", nameof(StopTouchingMyConfigs));
        }
    }
}
