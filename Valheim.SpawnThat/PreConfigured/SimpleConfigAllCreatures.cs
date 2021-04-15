using BepInEx;
using BepInEx.Configuration;
using System.IO;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.PreConfigured
{
    public static class SimpleConfigAllCreatures
    {
        public static void Initialize()
        {
            if (ConfigurationManager.GeneralConfig?.InitializeWithCreatures?.Value == true)
            {
                string configPath = Path.Combine(Paths.ConfigPath, ConfigurationManager.SimpleConfigFile);
                if (!File.Exists(configPath))
                {
                    var config = new SimpleConfigurationFile();

                    Log.LogDebug($"Initializing {ConfigurationManager.SimpleConfigFile}.");

                    ConfigFile file = new ConfigFile(configPath, true);

                    CreateEntry(file, config, "Crow");
                    CreateEntry(file, config, "FireFlies");
                    CreateEntry(file, config, "Deer");
                    CreateEntry(file, config, "Fish1");
                    CreateEntry(file, config, "Fish2");
                    CreateEntry(file, config, "Fish3");
                    CreateEntry(file, config, "Seagal");
                    CreateEntry(file, config, "Leviathan");
                    CreateEntry(file, config, "Boar");
                    CreateEntry(file, config, "Neck");
                    CreateEntry(file, config, "Greyling");
                    CreateEntry(file, config, "Greydwarf");
                    CreateEntry(file, config, "Greydwarf_Elite");
                    CreateEntry(file, config, "Greydwarf_shaman");
                    CreateEntry(file, config, "Troll");
                    CreateEntry(file, config, "Ghost");
                    CreateEntry(file, config, "Skeleton");
                    CreateEntry(file, config, "Skeleton_NoArcher");
                    CreateEntry(file, config, "Skeleton_poison");
                    CreateEntry(file, config, "Blob");
                    CreateEntry(file, config, "BlobElite");
                    CreateEntry(file, config, "Draugr");
                    CreateEntry(file, config, "Draugr_Ranged");
                    CreateEntry(file, config, "Draugr_Elite");
                    CreateEntry(file, config, "Leech");
                    CreateEntry(file, config, "Surtling");
                    CreateEntry(file, config, "Wraith");
                    CreateEntry(file, config, "Wolf");
                    CreateEntry(file, config, "Hatchling");
                    CreateEntry(file, config, "StoneGolem");
                    CreateEntry(file, config, "Fenring");
                    CreateEntry(file, config, "Deathsquito");
                    CreateEntry(file, config, "Lox");
                    CreateEntry(file, config, "Goblin");
                    CreateEntry(file, config, "GoblinArcher");
                    CreateEntry(file, config, "GoblinBrute");
                    CreateEntry(file, config, "GoblinShaman");
                    CreateEntry(file, config, "Serpent");

                    Log.LogDebug($"Finished initializing {ConfigurationManager.SimpleConfigFile}.");
                }
            }
        }

        private static void CreateEntry(ConfigFile file, SimpleConfigurationFile configFile, string creaturePrefab)
        {
            configFile.GetSubsection(creaturePrefab);
            var config = configFile.Subsections[creaturePrefab.Trim().ToUpper()];
            config.PrefabName.DefaultValue = creaturePrefab;
            config.PrefabName.Bind(file, creaturePrefab, nameof(SimpleConfig.PrefabName));
        }
    }
}
