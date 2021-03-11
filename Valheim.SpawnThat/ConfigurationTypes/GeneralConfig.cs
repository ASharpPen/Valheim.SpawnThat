using BepInEx.Configuration;

namespace Valheim.SpawnThat.ConfigurationTypes
{
    public class GeneralConfig
    {
        private ConfigFile Config;

        #region Debug

        public ConfigEntry<bool> DebugLoggingOn;

        public ConfigEntry<bool> TraceLoggingOn;

        public ConfigEntry<bool> WriteSpawnTablesToFileBeforeChanges;

        public ConfigEntry<bool> WriteSpawnTablesToFileAfterChanges;

        public ConfigEntry<bool> WriteCreatureSpawnersToFile;

        #endregion

        #region General

        #endregion

        public void Load(ConfigFile configFile)
        {
            Config = configFile;

            DebugLoggingOn = configFile.Bind<bool>("Debug", "DebugLoggingOn", false, "Enable debug logging.");
            TraceLoggingOn = configFile.Bind<bool>("Debug", "TraceLoggingOn", false, "Enables in-depth logging. Note, this might generate a LOT of log entries.");

            WriteSpawnTablesToFileBeforeChanges = configFile.Bind<bool>("Debug", "WriteSpawnTablesToFileBeforeChanges", false, "Dumps spawn system entries to a file, before applying custom changes.");
            WriteSpawnTablesToFileAfterChanges = configFile.Bind<bool>("Debug", "WriteSpawnTablesToFileAfterChanges", false, "Dumps spawn system entries to a file after applying configuration changes.");
            WriteCreatureSpawnersToFile = configFile.Bind<bool>("Debug", "WriteCreatureSpawnersToFile", false, "Dumps spawn system entries to a file after applying configuration changes.");
        }
    }
}
