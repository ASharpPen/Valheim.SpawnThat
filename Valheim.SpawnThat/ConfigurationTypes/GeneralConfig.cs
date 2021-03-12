using BepInEx.Configuration;

namespace Valheim.SpawnThat.ConfigurationTypes
{
    public class GeneralConfig
    {
        private ConfigFile Config;

        #region Debug

        public ConfigEntry<bool> DebugLoggingOn;

        public ConfigEntry<bool> TraceLoggingOn;

        #endregion

        #region General

        public ConfigEntry<bool> StopTouchingMyConfigs;

        #endregion

        #region WorldSpawner

        public ConfigEntry<bool> ClearAllExisting;

        public ConfigEntry<bool> AlwaysAppend;

        public ConfigEntry<bool> WriteSpawnTablesToFileBeforeChanges;

        public ConfigEntry<bool> WriteSpawnTablesToFileAfterChanges;

        #endregion

        #region LocalSpawner

        public ConfigEntry<bool> WriteCreatureSpawnersToFile;

        #endregion

        public void Load(ConfigFile configFile)
        {
            Config = configFile;

            DebugLoggingOn = configFile.Bind<bool>("Debug", "DebugLoggingOn", false, "Enable debug logging.");
            TraceLoggingOn = configFile.Bind<bool>("Debug", "TraceLoggingOn", false, "Enables in-depth logging. Note, this might generate a LOT of log entries.");

            WriteCreatureSpawnersToFile = configFile.Bind<bool>("LocalSpawner", "WriteSpawnTablesToFileBeforeChanges", false, "Dumps local spawners to a file before applying configuration changes.");

            ClearAllExisting = configFile.Bind<bool>("WorldSpawner", "ClearAllExisting", false, "If true, removes all existing world spawner templates.");
            AlwaysAppend = configFile.Bind<bool>("WorldSpawner", "AlwaysAppend", false, "If true, will never override existing spawners, but add all custom configurations to the list.");
            WriteSpawnTablesToFileBeforeChanges = configFile.Bind<bool>("WorldSpawner", "WriteSpawnTablesToFileBeforeChanges", false, "Dumps world spawner templates to a file, before applying custom changes.");
            WriteSpawnTablesToFileAfterChanges = configFile.Bind<bool>("WorldSpawner", "WriteSpawnTablesToFileAfterChanges", false, "Dumps world spawner templates to a file after applying configuration changes.");

            StopTouchingMyConfigs = configFile.Bind<bool>("General", nameof(StopTouchingMyConfigs), false, "Disables automatic updating and saving of drop table configurations.\nThis means no helpers will be added, but.. allows you to keep things compact.");
        }
    }
}
