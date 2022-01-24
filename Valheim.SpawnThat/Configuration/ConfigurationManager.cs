using BepInEx;
using BepInEx.Configuration;
using System.IO;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;

namespace Valheim.SpawnThat.Configuration
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
