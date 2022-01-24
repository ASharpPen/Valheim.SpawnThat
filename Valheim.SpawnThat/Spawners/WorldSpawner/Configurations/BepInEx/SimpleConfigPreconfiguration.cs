using BepInEx;
using BepInEx.Configuration;
using System.IO;
using System.Linq;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;

namespace Valheim.SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

// TODO: Generate this more dynamically, so we don't just have hardcoded version.
public static class SimpleConfigPreconfiguration
{
    public static void Initialize()
    {
        if (ConfigurationManager.GeneralConfig?.InitializeWithCreatures?.Value ?? false)
        {
            string configPath = Path.Combine(Paths.ConfigPath, SpawnSystemConfigurationManager.SimpleConfigFile);
            if (!File.Exists(configPath))
            {
                ConfigFile file = new ConfigFile(configPath, true);
                var config = new SimpleConfigurationFile();

                Log.LogDebug($"Initializing {SpawnSystemConfigurationManager.SimpleConfigFile}.");

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

                Log.LogDebug($"Finished initializing {SpawnSystemConfigurationManager.SimpleConfigFile}.");
            }
        }
    }

    private static void CreateEntry(ConfigFile file, string prefabName)
    {
        var config = new SimpleConfig();

        config.PrefabName.DefaultValue = prefabName;

        var entryType = typeof(IConfigurationEntry);

        foreach (var field in typeof(SimpleConfig).GetFields().Where(x => entryType.IsAssignableFrom(x.FieldType)))
        {
            var entry = field.GetValue(config) as IConfigurationEntry;
            entry.Bind(file, prefabName, field.Name);
        }
    }
}
