using BepInEx;
using BepInEx.Configuration;
using System.IO;
using Valheim.SpawnThat.ConfigurationTypes;

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
                    ConfigFile file = new ConfigFile(configPath, true);

                    CreateEntry(file, "Crow");
                    CreateEntry(file, "FireFlies");
                    CreateEntry(file, "Deer");
                    CreateEntry(file, "Fish1");
                    CreateEntry(file, "Fish2");
                    CreateEntry(file, "Fish3");
                    CreateEntry(file, "Seagal");
                    CreateEntry(file, "Leviathan");
                    CreateEntry(file, "Boar");
                    CreateEntry(file, "Neck");
                    CreateEntry(file, "Greyling");
                    CreateEntry(file, "Greydwarf");
                    CreateEntry(file, "Greydwarf_Elite");
                    CreateEntry(file, "Greydwarf_shaman");
                    CreateEntry(file, "Troll");
                    CreateEntry(file, "Ghost");
                    CreateEntry(file, "Skeleton");
                    CreateEntry(file, "Skeleton_NoArcher");
                    CreateEntry(file, "Skeleton_poison");
                    CreateEntry(file, "Blob");
                    CreateEntry(file, "BlobElite");
                    CreateEntry(file, "Draugr");
                    CreateEntry(file, "Draugr_Ranged");
                    CreateEntry(file, "Draugr_Elite");
                    CreateEntry(file, "Leech");
                    CreateEntry(file, "Surtling");
                    CreateEntry(file, "Wraith");
                    CreateEntry(file, "Wolf");
                    CreateEntry(file, "Hatchling");
                    CreateEntry(file, "StoneGolem");
                    CreateEntry(file, "Fenring");
                    CreateEntry(file, "Deathsquito");
                    CreateEntry(file, "Lox");
                    CreateEntry(file, "Goblin");
                    CreateEntry(file, "GoblinArcher");
                    CreateEntry(file, "GoblinBrute");
                    CreateEntry(file, "GoblinShaman");
                    CreateEntry(file, "Serpent");
                }
            }
        }

        private static void CreateEntry(ConfigFile file, string creaturePrefab)
        {
            var greydwarf = new SimpleConfig();
            greydwarf.PrefabName.DefaultValue = creaturePrefab;
            greydwarf.PrefabName.Bind(file, creaturePrefab, nameof(SimpleConfig.PrefabName));
        }
    }
}
