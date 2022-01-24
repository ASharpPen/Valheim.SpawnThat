using BepInEx;
using BepInEx.Configuration;
using SpawnThat.Configuration;
using System.IO;
using SpawnThat.Core;
using SpawnThat.Core.Configuration;

namespace SpawnThat.Configuration
{
    public static class ConfigurationManager
    {
        public static GeneralConfiguration GeneralConfig;

        internal const string GeneralConfigFile = "spawn_that.cfg";

        public static void LoadAllConfigurations()
        {
            GeneralConfig = LoadGeneral();
        }

        public static GeneralConfiguration LoadGeneral()
        {
            Log.LogDebug("Loading general configurations");

            string configPath = Path.Combine(Paths.ConfigPath, GeneralConfigFile);

            ConfigurationLoader.SanitizeSectionHeaders(configPath);
            ConfigFile configFile = new ConfigFile(configPath, true);

            var generalConfig = new GeneralConfiguration();
            generalConfig.Load(configFile);

            Log.LogDebug("Finished loading general configurations");

            return generalConfig;
        }
    }
}
